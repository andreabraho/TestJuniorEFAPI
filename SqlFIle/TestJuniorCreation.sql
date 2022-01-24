
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'TestJuniorDB')
BEGIN
    CREATE DATABASE TestJuniorDB
END
GO

USE TestJuniorDB
GO
DROP VIEW IF EXISTS dbo.UserReply
go

Drop INDEX IX_InsertDates_InsertDate ON dbo.InfoRequestReply
GO
DROP VIEW IF EXISTS dbo.NoCriticalDataAccount
go
DROP TABLE IF EXISTS dbo.InfoRequestReply
go
DROP TABLE IF EXISTS dbo.InfoRequest
go
DROP TABLE IF EXISTS dbo.Product_Category
go
DROP TABLE IF EXISTS dbo.Product
go
DROP TABLE IF EXISTS dbo.[User]
go
DROP TABLE IF EXISTS dbo.Category
go
DROP TABLE IF EXISTS dbo.Nation
go
DROP TABLE IF EXISTS dbo.Brand
go
DROP TABLE IF EXISTS dbo.Account
go

Create Table Category(
Id int IDENTITY ,
Name varchar(255) not null/*255 for byte reason*/
CONSTRAINT PK_Category PRIMARY KEY CLUSTERED (Id),
CONSTRAINT UNIQUE_Category_Name UNIQUE (Name),
CONSTRAINT CK_Category_Name_notEmpity CHECK (Name != '')
)
go

Create Table Nation(
Id int IDENTITY ,
Name varchar (255) not null,
CONSTRAINT PK_Nation PRIMARY KEY CLUSTERED (Id),
CONSTRAINT UNIQUE_Nation_Name UNIQUE (Name),
CONSTRAINT CK_Nation_Name_notEmpity CHECK (Name != '')

)
go

Create Table Account(
Id int IDENTITY ,
Email varchar(255) not null,
Password varchar(18) not null,
AccountType tinyint not null,
CONSTRAINT PK_Account PRIMARY KEY CLUSTERED (Id),
--CONSTRAINT UNIQUE_Account_Email UNIQUE(Email),
--CONSTRAINT CK_Account_Email_Valid CHECK (Email LIKE '%@%.%')
)
go

Create Table Brand(
Id int IDENTITY ,
AccountId int not null,
BrandName varchar(255)  not null,
Description varchar(255),
CONSTRAINT PK_Brand PRIMARY KEY CLUSTERED (Id)
)
go
ALTER TABLE Brand
ADD CONSTRAINT FK_Brand_AccountId
FOREIGN KEY (AccountId) REFERENCES Account(Id);
go

Create Table dbo.[User](
Id int IDENTITY,
AccountId int not null,
Name varchar(255) not null,
LastName varchar(255) not null,
CONSTRAINT PK_User PRIMARY KEY CLUSTERED (Id)
)
go
ALTER TABLE dbo.[User]
ADD CONSTRAINT FK_User_AccountId
FOREIGN KEY (AccountId) REFERENCES Account(Id);
go

Create Table Product(
Id int IDENTITY,
BrandId int not null,
Name varchar(255) not null,
ShortDescription nvarchar(255) not null,
Price decimal(18,2) not null,
Description nvarchar(max),
CONSTRAINT PK_Product PRIMARY KEY CLUSTERED (Id)

)
go
ALTER TABLE Product
ADD CONSTRAINT FK_Product_BrandId
FOREIGN KEY (BrandId) REFERENCES Brand(Id);
go

Create Table Product_Category(
ProductId int,
CategoryId int,
CONSTRAINT PK_ProductCategory PRIMARY KEY CLUSTERED (ProductId,CategoryId)
)
go
ALTER TABLE Product_Category
ADD CONSTRAINT FK_ProductCategory
FOREIGN KEY (ProductId) REFERENCES Product(Id);
go
ALTER TABLE Product_Category
ADD CONSTRAINT FK_CategoryProduct
FOREIGN KEY (CategoryId) REFERENCES Category(Id);
go

Create Table InfoRequest(
Id int IDENTITY ,
UserId int null,
ProductId int not null,
Name varchar(255) not null,
LastName varchar(255) not null ,
Email varchar(255) not null,
Citta  varchar(189),/*length of longest city name*/
NationId int,
Telefono varchar(15),/*The International Telecommunication Union (ITU) 15 digits*/
Cap varchar(18),/*Country with the longest zip code format is Chile with NNNNNNN, NNN-NNNN*/
RequestText varchar(255) not null,
InsertDate Date not null,
CONSTRAINT PK_InfoRequest PRIMARY KEY CLUSTERED (Id),
--CONSTRAINT CK_InfoRequest_Email_Valid CHECK (Email LIKE '%@%.%')


)
go
ALTER TABLE InfoRequest
ADD CONSTRAINT FK_InfoRequest_Product
FOREIGN KEY (ProductId) REFERENCES Product(Id);
go
ALTER TABLE InfoRequest
ADD CONSTRAINT FK_User_InfoRequest
FOREIGN KEY (UserId) REFERENCES dbo.[User](Id);
go
ALTER TABLE InfoRequest
ADD CONSTRAINT FK_Nation_InfoRequest
FOREIGN KEY (NationId) REFERENCES Nation(Id);
go

Create Table InfoRequestReply(
Id int IDENTITY,
InfoRequestId int not null,
AccountId int null,
ReplyText varchar(255) not null,
InsertDate Date not null,
CONSTRAINT PK_InfoRequestReply PRIMARY KEY CLUSTERED (Id)
)
go
ALTER TABLE InfoRequestReply
ADD CONSTRAINT FK_InfoRequestPreply_InfoRequest
FOREIGN KEY (InfoRequestId) REFERENCES InfoRequest(Id);
go
ALTER TABLE InfoRequestReply
ADD CONSTRAINT FK_InfoRequestPreply_Account
FOREIGN KEY (AccountId) REFERENCES Account(Id);
go
/*
@id input param that rappresents the id of a InfoRequestReply
RETURN:@user name of the user that is the author of the message,brandName or Name+LastName of the user
*/
CREATE OR ALTER FUNCTION GetUserOfInfoRequestReply (@id INT)
RETURNS varchar(666)
AS
BEGIN

	Declare @user Varchar(666)
	declare @infoRequestid INT
	declare @accountId Int
	Select @accountId=AccountId,@infoRequestid=InfoRequestId From InfoRequestReply Where Id=@id
	declare @accountType INT
	Select @accountType=AccountType From Account where Id=@accountId

	if(@accountType=2)
	begin
		Set @user= (Select Name From InfoRequest Where Id=@infoRequestid)+' '+(Select LastName From InfoRequest Where Id=@infoRequestid)
		
	end
	else
	begin 
		Set @user=(Select BrandName From Brand Where Brand.AccountId=@infoRequestid)
	end

RETURN @user
END
GO

/*
View with only no critical Account data 
*/
CREATE VIEW dbo.NoCriticalDataAccount
WITH SCHEMABINDING AS 
Select Id,AccountType From dbo.Account
GO

/*
View to have name of the user of a Reply and the Reply data
*/

CREATE VIEW dbo.UserReply 
 AS
Select InfoRequestReply.Id,
CASE Account.AccountType 
When 1 THEN (Select BrandName From dbo.Brand Where Brand.AccountId=InfoRequestReply.AccountId) 
ELSE (Select Name+' '+LastName FROM dbo.[User] Where [User].AccountId=InfoRequestReply.AccountId) 
END
as [User],AccountId,InfoRequestId,ReplyText,InsertDate
From dbo.InfoRequestReply JOIN dbo.Account On InfoRequestReply.AccountId=Account.Id
GO


/*
Function FOR 
● Dato un brand, recuperare l’elenco delle richieste informazioni per Product 
○ Id InfoRequest | IdProduct | Name Product | Name Richiedente | CogName
Richiedente | Testo richiesta | Data ultima risposta

@idBrand input param rappresents the brand that the Request list is returned

*/
CREATE OR ALTER FUNCTION BrandProductRequests (
    @idBrand INT
)
RETURNS TABLE
AS
RETURN
    Select InfoRequest.Id,InfoRequest.ProductId,Product.Name as NameProd,InfoRequest.Name,LastName,RequestText,T.MaxData
	From InfoRequest 
	Join 
	(Select InfoRequestId,MAX(InsertDate) as MaxData From InfoRequestReply Group By InfoRequestId) as T
	On InfoRequest.Id=T.InfoRequestId Join Product On InfoRequest.ProductId=Product.Id
	Join Brand on Product.BrandId=Brand.Id
	Where Brand.Id=@idBrand
GO	

/*
● Data una richiesta informazioni, elencare la cronologia delle risposte riportando i
seguenti dati:
○ User: campo che rappresenta o il Name e CogName dello User o NameBrand
della tabella Brand a seconda di chi ha risposto
○ ReplyText
○ InsertDate
*/

CREATE OR ALTER FUNCTION ReplyHistory (
    @idInfoRequest INT
)
RETURNS TABLE
AS
RETURN
    Select [User],ReplyText,InsertDate
	From UserReply 
	where UserReply.InfoRequestId=@idInfoRequest
GO

/*
Stored Procedure di paginazione Prodotti. la Stored Procedure prenderà in input i seguenti parametri
@NumProducts rappresents page size,throws exception if lower or equal than 0,
@NumPage number of wanted page,throws exception if lower or equal than 0,
@Category id of category,default0 for no filter,
@OrderBy: 1 product name | 2 priceAsc | 3 PriceDescending ,if differend leves default order
*/
CREATE OR ALTER PROCEDURE getPage   --easy mode
    @NumProducts INT,   
    @NumPage INT,
	@OrderBy INT,
	@Category INT=0
AS   
	if(@NumProducts<=0)
	throw 50000,'Products number can t be lower than 0',1
	if(@NumPage<=0)
	throw 50000,'Page number can t be lower than 0',1
    Select *
	From Product 
	WHERE Product.Id IN (Select ProductId FROM Product_Category Where @Category=0 OR CategoryId=@Category)
	ORDER BY CASE @OrderBy
				WHEN 1 THEN Name end,
			 CASE @OrderBy 
				WHEN 2 THEN Price END ASC,
			 CASE @OrderBy 
				WHEN 3 THEN Price END DESC
	OFFSET     @NumProducts*(@NumPage-1) ROWS       -- skip x rows
	FETCH NEXT @NumProducts ROWS ONLY;				-- take x rows
GO 
/*
Stored Procedure di paginazione Prodotti. la Stored Procedure prenderà in input i seguenti parametri
@NumProducts rappresents page size,throws exception if lower or equal than 0,
@NumPage number of wanted page,throws exception if lower or equal than 0,
@Category id of category,default0 for no filter,
@OrderBy: 1 product name | 2 priceAsc | 3 PriceDescending ,if differend leves default order
*/
CREATE OR ALTER PROCEDURE getPageV2 --normal mode
    @NumProducts INT,   
    @NumPage INT,
	@OrderBy INT,
	@Category INT=0
AS   
	if(@NumProducts<=0)
	throw 50000,'Products number can t be lower than 0',1
	if(@NumPage<=0)
	throw 50000,'Page number can t be lower than 0',1
	
    SELECT  *
	FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY CASE @OrderBy
												WHEN 1 THEN Name end,
											 CASE @OrderBy 
												WHEN 2 THEN Price END ASC,
											 CASE @OrderBy 
												WHEN 3 THEN Price END DESC ) AS RowNum, *
          FROM      Product
          WHERE Product.Id IN (Select ProductId FROM Product_Category Where @Category=0 OR CategoryId=@Category)
        ) AS RowConstrainedResult
	WHERE   RowNum >= (@NumProducts*(@NumPage-1)) AND	
			RowNum <  (@NumProducts*(@NumPage))
	ORDER BY RowNum			
GO


--exec getPageV2 -10,1,3,0

--select top 10 *
--from Product
--order by price desc

/*----INDEX CREATION-----*/



CREATE NONCLUSTERED INDEX IX_InsertDates_InsertDate 
ON dbo.InfoRequestReply (InfoRequestId)
INCLUDE (ReplyText,InsertDate)

CREATE NONCLUSTERED INDEX IX_AccountTypes_AccountType
ON dbo.InfoRequestReply (AccountId)

CREATE NONCLUSTERED INDEX IX_InserDates_InsertDate
ON dbo.InfoRequestReply (InsertDate)


CREATE NONCLUSTERED INDEX IX_InfoRequestId_ProductId 
On dbo.InfoRequest(ProductId)
INCLUDE ([Name],LastName,RequestText)

USE [TestJuniorDB]
GO
CREATE NONCLUSTERED INDEX IX_Product_BrandIdPrice
ON [dbo].[Product] ([BrandId],[Price])
INCLUDE ([Name])
GO

