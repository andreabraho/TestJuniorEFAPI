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
Declare @x INT =@MaxProduct-@MinProduct/*used for random numbers*/

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

declare @rndNum INT
DECLARE @Counter1 INT 

/*Creating 20 categories*/
SET @Counter=1
while (@Counter<=@CategoryCreated)
begin 
	INSERT INTO Category (Name)
	VALUES ('category '+CAST(@Counter as varchar(2)))
	SET @Counter=@Counter+1
end
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
--creating user
SET @Counter = 1
WHILE (@Counter <= @UserCreated)
BEGIN
	INSERT INTO Account (Email,Password,AccountType)
	VALUES ('email'+CAST(@Counter as varchar(2))+'@gmail.com',
			'password'+CAST(@Counter as varchar (2)),2)

	INSERT INTO [User] (AccountId,Name,LastName)
	VALUES (SCOPE_IDENTITY(),'Name '+CAST(@Counter as varchar(2)),
			'LastName '+CAST(@Counter as varchar(2)))
	SET @Counter = @Counter  + 1
END
--creating brand,products,productCategory,infoRequest,infoRequestReply

/*
○ Per ognuno dei 50 Account Brand creare in modo randomico da 10 a 50 Prodotti
○ Ciascun Product deve essere associato da 0 a 5 categorie fra le 20 inserite in
modo randomico dallo script
○ Ad ogni Product devono essere associate da 0 a 10 richieste informazioni da
parte di utenti Guest o utenti Registrati.
○ Per ogni richiesta informazioni generare una sequenza di 1-3 risposte da parte
dell’azienda e dell’utente*/


SET @Counter = 1
WHILE (@Counter <= @BrandCreated)
BEGIN
    INSERT INTO Account (Email,Password,AccountType)
	VALUES ('email'+CAST(@Counter as varchar(2))+'@gmail.com',
			'password'+CAST(@Counter as varchar (2)),1)
	
	INSERT INTO Brand (AccountId,BrandName,Description)
	VALUES (SCOPE_IDENTITY(),'Brand Name '+CAST(@Counter as varchar(2)),
			'Brand Description '+CAST(@Counter as varchar(2)))

	declare @lastBrand INT=SCOPE_IDENTITY()--saving last brand for insert products

	SELECT @rndNum =ROUND(RAND()*@x+@MinProduct,0)--num of products to be created fo
	SET @Counter1=1
	WHILE (@Counter1<@rndNum)--start while insert products
	BEGIN
		INSERT INTO Product (BrandId,Name,ShortDescription,Price,Description)
				VALUES (@lastBrand,'name '+CAST(@Counter1 as varchar(2)),
				'short description '+CAST(@Counter1 as varchar(2)),
				(RAND()*400+10),
				'description '+CAST(@Counter1 as varchar(2)))
		declare @lastProduct INT =SCOPE_IDENTITY()

		declare @rndNum2 INT =ROUND(RAND()*@MaxInfoRequestCreated,0)
		declare @counter2 INT=1
		WHILE (@counter2<=@rndNum2)--while creating InfoRequest
		BEGIN
			declare @rndNumX INT
			SELECT @rndNumX = ROUND((RAND()*5),0)/*random for deciding if the request has a user id or not*/
			declare @userId INT
			Select @userId=Id From [User] Order By NEWID()

			IF (@rndNumX=0)/* if 0 has no user id */
				INSERT INTO InfoRequest (UserId,ProductId,Name,LastName
					,Email,Citta,NationId,Telefono,Cap,RequestText,InsertDate)
				VALUES(NULL,@lastProduct,'Name','LastName','email'
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
				VALUES(@userId,@lastProduct
				,@userName,@userLastName
				,'email'
					,'citta',(Select top 1 Id From Nation Order by NEWID()),'telefono'
					,'cap','testo richiesta',DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0))
				end
			declare @lastInfoRequest INT =SCOPE_IDENTITY()

			declare @counter3 INT =1
			declare @rndNum3 INT=ROUND((RAND()*@MaxInfoRequestReply+ 1),0)
			WHILE(@counter3<=@rndNum3)--start while for creating info Request Reply
			BEGIN	
				
				declare @idAccount INT

				if(ROUND((RAND()*2),0)=0)--rnd for deciding if the reply is from user or brand
					Select @idAccount=Account.Id
					FROM InfoRequest JOIN [User] On InfoRequest.UserId=[User].Id Join Account ON [User].AccountId=Account.Id
					WHERE InfoRequest.Id=@lastInfoRequest
				else
					Select @idAccount=Account.Id 
					FROM Account JOIN BRAND on Account.Id=Brand.AccountId Join Product ON Brand.Id=Product.BrandId Join InfoRequest On Product.Id=InfoRequest.Id
					WHERE InfoRequest.Id=@lastInfoRequest


				INSERT INTO InfoRequestReply(InfoRequestId,AccountId,ReplyText,InsertDate)
				VALUES (@lastInfoRequest,@idAccount,CONVERT(varchar(255), NEWID()),DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0))
				
				SET @Counter3=@Counter3+1
			END--end while InfoRequest Reply

			SET @Counter2=@Counter2+1
		END--end while creation info Request

		declare @rndNum4 INT=ROUND(RAND()*@MaxCatAssociatedToProduct,0)
		declare @counter4 INT =1
		While(@counter4<=@rndNum4)--start while connecting categories
		BEGIN
			INSERT INTO Product_Category
			VALUES (@lastProduct,(Select top 1 Id From Category Where Id not In (Select CategoryId From Product_Category Where ProductId=@lastProduct)))/*NO DUPLICATE*/
		
			SET @Counter4=@Counter4+1
		END--end while connecting categories

		Set @Counter1=@Counter1+1
	END--end product creation while

	SET @Counter = @Counter  + 1
END--end brand creation while


