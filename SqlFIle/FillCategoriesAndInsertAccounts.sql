Use TestJuniorDB
Go
/*prendere tutti i prodotti senza categoria e associarne almeno una random*/

Declare @Id INT,
		@Counter INT

DECLARE NoCatProducts_cursor  CURSOR FORWARD_ONLY FOR 
	SELECT Id
	FROM Product
	Where Product.Id not in (Select ProductId From Product_Category)

OPEN NoCatProducts_cursor

	FETCH NEXT FROM NoCatProducts_cursor
		INTO @Id

	IF(@@ROWCOUNT>0)
	BEGIN
		WHILE @@FETCH_STATUS = 0
		BEGIN--cursor cicle
		
			Insert Into Product_Category 
			Values(@Id,(Select Top 1 Id From Category Order by NEWID()))

			FETCH NEXT FROM NoCatProducts_cursor
					INTO @Id
		END          
		
			
	END

CLOSE NoCatProducts_cursor
DEALLOCATE NoCatProducts_cursor
GO

Select *
From Product Left Join Product_Category On Product.Id=Product_Category.ProductId
Where Product_Category.CategoryId is Null
GO
/*individuare gli utenti guest e registrarli al sito come account utenti*/
Declare @name Varchar(255),
		@lastName varchar(255),
		@email varchar(255),
		@Counter INT=1,
		@lastAccount INT,
		@lastUser INT,
		@irId INT

DECLARE GuestUsers_cursor CURSOR FORWARD_ONLY FOR 
	SELECT Id,Name,LastName,Email
	From InfoRequest
	Where UserId is NULL

OPEN GuestUsers_cursor

	FETCH NEXT FROM GuestUsers_cursor
		INTO @irId,@name,@lastName,@email

	IF(@@ROWCOUNT>0)
	BEGIN
		WHILE @@FETCH_STATUS = 0
		BEGIN--cursor cicle to regiser the possibly not registered account

			IF(not EXISTS(SELECT top (1) * FROM Account Where Account.Email=@email))--if not exists an account with the email taken from possibly not registered account then register the account
			BEGIN
				Insert Into Account --creating the new account of type user
				Values (@email,SUBSTRING(CAST(NEWID() as varchar(36)), 0 , 17),2)

				Set @lastAccount =SCOPE_IDENTITY()

				INSERT INTO [User] --filling user
				Values (@lastAccount,@name,@lastName)
				SET @lastUser=SCOPE_IDENTITY()
				UPDATE InfoRequest--update guest info request with the newly created account
				Set UserId=@lastUser
				Where Email=@email
			END
			---do else
			ELSE --there is an account with the email so update the userId of infoRequests
			BEGIN
			UPDATE InfoRequest
			Set UserId=(SELECT top (1) Id FROM Account Where Account.Email=@email)
			--Where Email=@email AND UserId is null performance problem
			WHERE Id=@irId
			END
			FETCH NEXT FROM GuestUsers_cursor
			INTO @irId,@name,@lastName,@email
		END          
		
			
	END

CLOSE GuestUsers_cursor
DEALLOCATE GuestUsers_cursor
GO

