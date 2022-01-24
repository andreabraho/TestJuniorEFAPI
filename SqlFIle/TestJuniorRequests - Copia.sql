/*
Successivamente sono richieste le seguenti proiezioni/statistiche di dati:
*/

/*
● Numero delle richieste informazioni raccolte per Product
	○ Name Product | Num Richieste
*/
USE TestJuniorDB
GO

Select T.NameProduct,T.NumRequests
From(
Select Product.Id,Product.Name as NameProduct,COUNT(*) as NumRequests
From Product inner Join InfoRequest On Product.Id=InfoRequest.ProductId
Group By Product.Id,Product.Name
) AS T

Select T.NameProduct,T.NumRequests
From(
Select Product.Id,Product.Name as NameProduct,COUNT(*) as NumRequests
From InfoRequest inner Join Product On InfoRequest.ProductId=Product.Id
Group By Product.Id,Product.Name
) AS T


SELECT D.Name,T.NumRequests
FROM (
Select Product.Id ,COUNT(*) as NumRequests
From Product inner Join InfoRequest On Product.Id=InfoRequest.ProductId
Group By Product.Id
) AS T INNER JOIN 
(SELECT Product.Id,Product.Name
FROM PRODUCT) AS D
ON T.Id=D.Id
/*
● Numero delle richieste informazioni raccolte per Brand
○ Name Brand | Num Richieste
*/
Select T.BrandName,T.NumRequests
From(
Select Brand.Id,Brand.BrandName,COUNT(Product.Id) as NumRequests
FROM Brand Inner Join Product On Brand.Id=Product.BrandId 
		Inner Join InfoRequest On Product.Id=InfoRequest.ProductId
Group By Brand.Id,Brand.BrandName
) as T

--Select b.BrandName,T.NumRequests
--From(
--Select Brand.Id,COUNT(Product.Id) as NumRequests
--FROM Brand Join Product On Brand.Id=Product.BrandId 
--		Join InfoRequest On Product.Id=InfoRequest.ProductId
--Group By Brand.Id
--) as T 
--JOIN 
--Brand as b On T.Id=b.Id

/*
● Numero delle richieste informazioni raccolte per Product riportando anche il Name del
Brand
○ Name Brand | Name Product | Num Richieste
*/

Select Brand.BrandName,ProdNumRichieste.NameProduct,ProdNumRichieste.NumRequests
From (
Select Product.Id,Product.Name as NameProduct,COUNT(InfoRequest.Id) as NumRequests,BrandId
From Product left Join InfoRequest On Product.Id=InfoRequest.ProductId 
Group By Product.Id,Product.Name,BrandId
)as ProdNumRichieste Inner Join Brand On ProdNumRichieste.BrandId=Brand.Id

/*
● Numero dei prodotti per Categoria di ciascun Brand:
○ Name Brand | Name Categoria | Numero Prodotti
*/

Select BrandId,CategoryId,COUNT(Product.Id)as NumProducts
FROM Brand 
Inner JOIN Product ON Brand.Id=Product.BrandId 
Inner Join Product_Category On Product.Id=Product_Category.ProductId
Group BY BrandId,CategoryId
Order BY BrandId

Select BrandName,Category.Name,NumProducts
FROM
(
Select BrandId,CategoryId,COUNT(Product.Id)as NumProducts
FROM Brand 
Inner JOIN Product ON Brand.Id=Product.BrandId 
Inner Join Product_Category On Product.Id=Product_Category.ProductId
Group BY BrandId,CategoryId
)as T
Inner JOIN Brand ON T.BrandId =Brand.Id
Inner JOIN Category ON T.CategoryId=Category.Id
Order BY BrandName

/*
● Elenco dei prodotti con più di una categoria associata
○ Name Product | Num Categorie
*/
Select t.Name AS NameProduct,t.NumCatAssociated
From(
Select Product.Id,Product.Name,Count(Product_Category.ProductId) as NumCatAssociated
From Product Inner Join Product_Category ON Product.Id=Product_Category.ProductId
Group By Product.Id,Product.Name
HAVING Count(Product_Category.ProductId)>1
) as t
/*
● Elenco dei prodotti con nessuna categoria associata
*/
Select Tab.Id,Tab.Name
FROM(
Select Product.Id,Product.Name,Count(Product_Category.ProductId) as NumCatAssociated
From Product left  Join Product_Category ON Product.Id=Product_Category.ProductId
Group By Product.Id,Product.Name
HAVING Count(Product_Category.ProductId)=0
)AS Tab

Select Product.Id,Product.Name
From Product left  Join Product_Category ON Product.Id=Product_Category.ProductId
where ProductId IS NULL



/*
● Numero dei prodotti per Brand, ordinata per numero dei prodotti decrescente
○ Name Brand | Num Prodotti
*/
Select T.BrandName,T.NumProducts
From(
Select Brand.Id,Brand.BrandName,COUNT(Product.Id) as NumProducts
FROM Brand Inner Join Product On Brand.Id=Product.BrandId
GROUP BY Brand.Id,Brand.BrandName
)as T
ORDER BY T.NumProducts
/*
● Prezzo medio dei prodotti per Brand, ordinata del prezzo medio più alto al più basso
○ Name Brand | Prezzo Medio Prodotti
*/
Select T.BrandName,T.AvgPrice
From(
Select Brand.Id,Brand.BrandName,AVG(Product.Price) as AvgPrice
FROM Brand Inner Join Product On Brand.Id=Product.BrandId
GROUP BY Brand.Id,Brand.BrandName
)AS T
ORDER BY AvgPrice DESC

/*
● Il Product più caro di ciascun Brand con il rispettivo Name Product
○ Name Brand | Name Product | Prezzo
*/

Select Brand.BrandName,Product.Name,Product.Price
FROM Brand Inner Join Product On Brand.Id=Product.BrandId
Where Product.Price=(Select Max(Price)
						FROM Product as p
						Where p.BrandId=Brand.Id)


/*
● Il Product con il prezzo più alto e il Product con il prezzo e più basso per ciascun Brand
con i rispettivi nomi Product
○ Name Brand | Name Product | Prezzo | Name Product | Prezzo
*/


Select MaxPrices.BrandName,MaxPrices.Name,MaxPrices.Price,MinPrices.Name,MinPrices.Price
FROM (
Select Brand.Id,Brand.BrandName,Product.Name,Product.Price
FROM Brand Inner Join Product On Brand.Id=Product.BrandId
Where Product.Price=(Select Max(Price)
						FROM Product as p
						Where p.BrandId=Brand.Id)
) AS MaxPrices
Inner JOIN 
(
Select Brand.Id,Brand.BrandName,Product.Name,Product.Price
FROM Brand Inner Join Product On Brand.Id=Product.BrandId
Where Product.Price=(Select min(Price)
						FROM Product as p
						Where p.BrandId=Brand.Id)
)As MinPrices
On MaxPrices.Id=MinPrices.Id

/*
● Data una richiesta informazioni, elencare la cronologia delle risposte riportando i
seguenti dati:
○ User: campo che rappresenta o il Name e CogName dello User o NameBrand
della tabella Brand a seconda di chi ha risposto
○ ReplyText
○ InsertDate
*/

Select dbo.GetUserOfInfoRequestReply(Id) as [User],ReplyText,InsertDate
From InfoRequestReply
where InfoRequestReply.InfoRequestId=(Select Top 1 InfoRequestId From InfoRequestReply Order BY NEWID())
Order By InsertDate desc


/*V2*/
Select [User],ReplyText,InsertDate
From UserReply 
where UserReply.InfoRequestId=(Select Top 1 InfoRequestId From InfoRequestReply Order BY NEWID())
Order By InsertDate desc
/*V3*/

Select *
From ReplyHistory((Select Top 1 InfoRequestId From InfoRequestReply Order BY NEWID()))
Order By InsertDate desc

/*
● Dato un brand, recuperare l’elenco delle richieste informazioni per Product ordinate per
ultima risposta più recente
○ Id InfoRequest | IdProduct | Name Product | Name Richiedente | CogName
Richiedente | Testo richiesta | Data ultima risposta
*/

Select InfoRequest.Id,InfoRequest.ProductId,Product.Name as NameProd,InfoRequest.Name,LastName,RequestText,T.MaxData
From InfoRequest 
Inner Join 
(Select InfoRequestId,MAX(InsertDate) as MaxData From InfoRequestReply Group By InfoRequestId) as T
On InfoRequest.Id=T.InfoRequestId Inner Join Product On InfoRequest.ProductId=Product.Id
Inner Join Brand on Product.BrandId=Brand.Id
Where Brand.Id=(Select Top 1 Id From Brand Order BY NEWID())
Order BY T.MaxData Desc

/*V2*/
Select *
From BrandProductRequests((Select Top 1 Id From Brand Order BY NEWID())) AS T
Order BY T.MaxData Desc


/*---------------------------------*/

/*Nested loops join test*/
Select * 
From Nation as N INNER JOIN InfoRequest as IR ON N.Id=IR.NationId
Where N.Id=1

Select * 
From InfoRequest as IR INNER JOIN Nation as N ON IR.NationId=N.Id
Where N.Id=1

Select * 
From (Select * From Nation Where Id=1) AS N INNER JOIN InfoRequest as IR ON N.Id=IR.NationId


