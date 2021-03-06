/*
Creare uno script T-SQL di seed dei dati di test in cui vengono creati:
*/

use TestJuniorDB



IF(not EXISTS(SELECT top (1) * FROM dbo.InfoRequestReply))
begin
DBCC CHECKIDENT ('InfoRequestReply', RESEED , 1)
end
else
begin
DELETE FROM InfoRequestReply
DBCC CHECKIDENT ('InfoRequestReply', RESEED , 0)
end

IF(not EXISTS(SELECT top (1) * FROM dbo.InfoRequest))
begin
DBCC CHECKIDENT ('InfoRequest', RESEED , 1)
end
else
begin
DELETE FROM InfoRequest
DBCC CHECKIDENT ('InfoRequest', RESEED , 0)
end

DELETE FROM Product_Category

IF(not EXISTS(SELECT top (1) * FROM dbo.Category))
begin
DBCC CHECKIDENT ('Category', RESEED , 1)
end
else
begin
DELETE FROM Category
DBCC CHECKIDENT ('Category', RESEED , 0)
end

IF(not EXISTS(SELECT top (1) * FROM dbo.Nation))
begin
DBCC CHECKIDENT ('Nation', RESEED , 1)
end
else
begin
DELETE FROM Nation
DBCC CHECKIDENT ('Nation', RESEED , 0)
end

IF(not EXISTS(SELECT top (1) * FROM dbo.Product))
begin
DBCC CHECKIDENT ('Product', RESEED , 1)
end
else
begin
DELETE FROM Product
DBCC CHECKIDENT ('Product', RESEED , 0)
end

IF(not EXISTS(SELECT top (1) * FROM dbo.Brand))
begin
DBCC CHECKIDENT ('Brand', RESEED , 1)
end
else
begin
DELETE FROM Brand
DBCC CHECKIDENT ('Brand', RESEED , 0)
end

IF(not EXISTS(SELECT top (1) * FROM dbo.[User]))
begin
DBCC CHECKIDENT ('[User]', RESEED , 1)
end
else
begin
DELETE FROM [User]
DBCC CHECKIDENT ('[User]', RESEED , 0)
end


IF(not EXISTS(SELECT top (1) * FROM dbo.Account))
begin
DBCC CHECKIDENT ('Account', RESEED , 1)
end
else
begin
DELETE FROM Account
DBCC CHECKIDENT ('Account', RESEED , 0)
end

/*
○ 100 Account, dei quali 50 Brand e 50 User;
*/
Declare @BrandCreated INT =50
Declare @UserCreated INT=50

/*------------------
min and max products to be created for each brand*/
Declare @MaxProduct INT =50
Declare @MinProduct INT =10
/*----------------------------------*/

Declare @CategoryCreated INT=20
Declare @NationCreated INT=20
/*info request created for each product*/
Declare @MaxInfoRequestCreated INT=10 
/*max category that can be associated to a product*/
Declare @MaxCatAssociatedToProduct INT = 5
/*max info request reply for each info request*/
Declare @MaxInfoRequestReply INT =3

DECLARE @Counter INT 



SET @Counter = 1
WHILE (@Counter <= @BrandCreated)
BEGIN
    INSERT INTO Account (Email,Password,AccountType)
	VALUES ('email'+CAST(@Counter as varchar(2))+'@gmail.com',
			'password'+CAST(@Counter as varchar (2)),1)
	
	INSERT INTO Brand (AccountId,BrandName,Description)
	VALUES (@Counter,'Brand Name '+CAST(@Counter as varchar(2)),
			'Brand Description '+CAST(@Counter as varchar(2)))

	SET @Counter = @Counter  + 1
END



SET @Counter = 1
WHILE (@Counter <= @UserCreated)
BEGIN
	INSERT INTO Account (Email,Password,AccountType)
	VALUES ('email'+CAST(@Counter as varchar(2))+'@gmail.com',
			'password'+CAST(@Counter as varchar (2)),2)

	INSERT INTO [User] (AccountId,Name,LastName)
	VALUES (@Counter+@BrandCreated,'Name '+CAST(@Counter as varchar(2)),
			'LastName '+CAST(@Counter as varchar(2)))
	SET @Counter = @Counter  + 1
END

/*
○ Per ognuno dei 50 Account Brand creare in modo randomico da 10 a 50 Prodotti
*/


Declare @x INT =@MaxProduct-@MinProduct/*used for random numbers*/

declare @idColumn int

select @idColumn = min( Id ) from Brand

while @idColumn is not null
begin
    declare @rndNum INT
	DECLARE @Counter1 INT 
	SET @Counter1 = 1
	SELECT @rndNum = ROUND((RAND()*@x+@MinProduct),0)
		WHILE (@Counter1 <= @rndNum)
			BEGIN

				INSERT INTO Product (BrandId,Name,ShortDescription,Price,Description)
				VALUES (@idColumn,'name '+CAST(@Counter1 as varchar(2)),
				'short description '+CAST(@Counter1 as varchar(2)),
				(RAND()*400+10),
				'description '+CAST(@Counter1 as varchar(2)))

				SET @Counter1 = @Counter1  + 1
			END
    select @idColumn = min( Id ) from Brand where Id > @idColumn
end

/*
○ Ciascun Product deve essere associato da 0 a 5 categorie fra le 20 inserite in
modo randomico dallo script
*/

/*Creating 20 categories*/
SET @Counter=1
while (@Counter<=@CategoryCreated)
begin 
	INSERT INTO Category (Name)
	VALUES ('category '+CAST(@Counter as varchar(2)))
	SET @Counter=@Counter+1
end

/*Connecting */
select @idColumn = min( Id ) from Product

while @idColumn is not null
begin
    
	SET @Counter1 = 1
	SELECT @rndNum = ROUND((RAND()*@MaxCatAssociatedToProduct),0)/*random for category association*/

	while (@Counter1<=@rndNum)
	begin
		INSERT INTO Product_Category
		VALUES (@idColumn,(Select top 1 Id From Category Where Id not In (Select CategoryId From Product_Category Where ProductId=@idColumn)))/*NO DUPLICATE*/
		SET @Counter1=@Counter1+1
	end
    select @idColumn = min( Id ) from Product where Id > @idColumn
end

/*
○ Ad ogni Product devono essere associate da 0 a 10 richieste informazioni da
parte di utenti Guest o utenti Registrati.
*/

/*
Creating Nations
*/

SET @Counter=1
while (@Counter<=@NationCreated)
begin
	INSERT INTO Nation(Name)
	VALUES('Nation '+CAST(@Counter as varchar(2)))
	SET @Counter=@Counter+1
end


select @idColumn = min( Id ) from Product

while @idColumn is not null
begin
    
	SET @Counter1 = 1
	SELECT @rndNum = ROUND((RAND()*@MaxInfoRequestCreated),0)
	while (@Counter1<=@rndNum)

	begin
		declare @rndNum2 INT
		SELECT @rndNum2 = ROUND((RAND()*5),0)/*random for deciding if the request has a user id or not*/
		declare @userId INT
		Select @userId=Id From [User] Order By NEWID()

		IF (@rndNum2=0)/* if 0 has no user id */
			INSERT INTO InfoRequest (UserId,ProductId,Name,LastName
				,Email,Citta,NationId,Telefono,Cap,RequestText,InsertDate)
			VALUES(NULL,@idColumn,'Name','LastName','email'
				,'citta',(Select top 1 Id From Nation Order by NEWID()),'telefono'
				,'cap','testo richiesta',DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0))
		ELSE
			begin

			
			declare @userName VARCHAR(333)
			SELECT @userName=Name From [User] where Id=@userId
		
			declare @userLastName VARCHAR(333)
			SELECT @userLastName=LastName From [User] where Id=@userId

			INSERT INTO InfoRequest (UserId,ProductId,Name,LastName
				,Email,Citta,NationId,Telefono,Cap,RequestText,InsertDate)
			VALUES(@userId,@idColumn
			,@userName,@userLastName
			,'email'
				,'citta',(Select top 1 Id From Nation Order by NEWID()),'telefono'
				,'cap','testo richiesta',DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0))
			end
		SET @Counter1=@Counter1+1
	end
    select @idColumn = min( Id ) from Product where Id > @idColumn
end


/*○ Per ogni richiesta informazioni generare una sequenza di 1-3 risposte da parte
dell’azienda e dell’utente*/

select @idColumn = min( Id ) from InfoRequest

while @idColumn is not null
begin
    
	SET @Counter1 = 1
	SELECT @rndNum = ROUND((RAND()*(@MaxInfoRequestReply-1)+1),0)
	while (@Counter1<=@rndNum)
	begin
		DECLARE @rndNum1 INT
		declare @idAccount INT

		if(ROUND((RAND()*2),0)=0)
			Select @idAccount=Account.Id
			FROM InfoRequest JOIN [User] On InfoRequest.UserId=[User].Id Join Account ON [User].AccountId=Account.Id
			WHERE InfoRequest.Id=@idColumn
		else
			Select @idAccount=Account.Id 
			FROM Account JOIN BRAND on Account.Id=Brand.AccountId Join Product ON Brand.Id=Product.BrandId Join InfoRequest On Product.Id=InfoRequest.Id
			WHERE InfoRequest.Id=@idColumn


		INSERT INTO InfoRequestReply(InfoRequestId,AccountId,ReplyText,InsertDate)
		VALUES (@idColumn,@idAccount,CONVERT(varchar(255), NEWID()),DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0))
		SET @Counter1=@Counter1+1


	end
    select @idColumn = min( Id ) from InfoRequest where Id > @idColumn
end

