USE TestJuniorDB
GO
/*
Creazione di una Stored Procedure di paginazione Prodotti. la Stored Procedure prenderà in input i seguenti parametri
dimensione della pagina,
il numero di pagina,
categoria,
ordinamento: 1 nome prodotto | 2 prezzo crescente | 3 prezzo decrescente
*/
/*
procedure to get a page of products
@NumProducts=page size
@NumPage=number of page needed
@OrderBy 1for order by name 2for acending order 3 for descendind order other for no order
@Category default 0 for no filter, category id for a filter on category
*/

CREATE OR ALTER PROCEDURE getPage   --easy mode
    @NumProducts INT,   
    @NumPage INT,
	@OrderBy INT,
	@Category INT
AS   

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
procedure to get a page of products
@NumProducts=page size
@NumPage=number of page needed
@OrderBy 1for order by name 2for acending order 3 for descendind order other for no order
@Category default 0 for no filter, category id for a filter on category
*/
CREATE OR ALTER PROCEDURE getPageV2 --normal mode
    @NumProducts INT,   
    @NumPage INT,
	@OrderBy INT,
	@Category INT
AS   

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


/*
Test Stored Procedure di paginazione Prodotti.
numProduct
numPage
orderBy
Category
*/

EXEC getPage 10,1,1,1
EXEC getPageV2 10,1,1,1
EXEC getPage 10,1,1,1
EXEC getPage 10,3,1,1


/*
i primi tre più costosi del brand   ordinate per brand e prezzo
Nome brand | Nome prodotto | Prezzo
*/

select * from (
    select BrandName, 
           Product.Name as ProductName,
           Price, 
           rank() over (partition by Brand.Id order by price desc) as price_rank 
    from Product Join Brand ON Product.BrandId=Brand.Id) TopProducts
where price_rank <= 3;
GO

select * from (
    select BrandName, 
           Product.Name as ProductName,
           Price, 
           row_number() over (partition by Brand.Id order by price desc) as price_rank 
    from Product Join Brand ON Product.BrandId=Brand.Id) TopProducts
where price_rank <= 3;
GO

SELECT  t1.BrandName, t2o.Name as ProductName,t2o.Price
FROM    Brand as t1
CROSS APPLY
        (
        SELECT  TOP 3 *
        FROM    Product as t2
        WHERE   t2.BrandId = t1.Id
        ORDER BY
                t2.Price DESC
        ) t2o

SELECT  t1.BrandName, t2o.Name as ProductName,t2o.Price
FROM    Brand as t1
Outer APPLY
        (
        SELECT  TOP 3 *
        FROM    Product as t2
        WHERE   t2.BrandId = t1.Id
        ORDER BY
                t2.Price DESC
        ) t2o
/*
Restituire il seguente elenco prodotti con ordine Custom:
* come primi, i 20 più recenti,
* come seconda fascia i primi 100 prodotti con più richieste informazioni ricevute
* come terza fascia, i 10 con prezzo compreso tra 200 - 500€
* come quarta fascia, i 100 con nessuna richiesta informazione
*/

--with version union all
	WITH Recent20 AS (
		Select top(20) *
		From Product
		Order By Id Desc
	),MostRequested100 AS(
		Select  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description
		From(
		Select top (100) Product.Id,Product.Name as NameProduct
		From Product inner Join InfoRequest On Product.Id=InfoRequest.ProductId
		Group By Product.Id,Product.Name
		order by COUNT(*) Desc
		) AS T INNER JOIN Product as P ON T.Id=P.Id
	),HighPrice10 AS(
		Select top(10) *
		From Product
		Where Price>200 AND Price <500
		order by Id
	),LowRequested100 AS (
		Select TOP(100)  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description
		From Product as P Left JOIN InfoRequest ON P.Id=InfoRequest.ProductId
		Where InfoRequest.Id is NUll
	)
	Select *
	FROM Recent20
	UNION ALL
	Select *
	FROM MostRequested100
	UNION ALL
	Select *
	FROM HighPrice10
	UNION ALL
	Select *
	FROM LowRequested100

--V1
/*test adding custom prod in a temp table*/
	DROP TABLE IF EXISTS #CustomProd 

	Create Table #CustomProd(
	Id int,
	BrandId int,
	Name varchar(255),
	ShortDescription varchar(255),
	Price decimal(18,2),
	Description varchar(max)
	)

	Insert Into #CustomProd
	Select top(20) * 
	From Product 
	Order By Id Desc;

	Insert Into #CustomProd
	Select top (100) P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description
	From(
	Select Product.Id,Product.Name as NameProduct,COUNT(*) as NumRequests
	From Product inner Join InfoRequest On Product.Id=InfoRequest.ProductId
	Group By Product.Id,Product.Name
	) AS T INNER JOIN Product as P ON T.Id=P.Id
	Order By NumRequests

	Insert Into #CustomProd
	Select top(10) *
	From Product
	Where Price>200 AND Price <500
	Order BY NEWID()

	Insert Into #CustomProd
	Select TOP(100)  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description
	From Product as P Left JOIN InfoRequest ON P.Id=InfoRequest.ProductId
	Where InfoRequest.Id is NUll

	Select * From #CustomProd
--V2 union normal with position
	Select *
	From 
	(
	Select top(20) *,ROW_NUMBER() OVER (Order By Id Desc) as Position
	From Product

	UNION 

	Select  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description,ROW_NUMBER() OVER (Order By NumRequests)+20 as Position
	From(
	Select top (100) Product.Id,Product.Name as NameProduct,COUNT(*) as NumRequests
	From Product inner Join InfoRequest On Product.Id=InfoRequest.ProductId
	Group By Product.Id,Product.Name
	order by COUNT(*)
	) AS T INNER JOIN Product as P ON T.Id=P.Id


	UNION 

	Select top(10) *,ROW_NUMBER() OVER (Order BY Id)+120 as Position
	From Product
	Where Price>200 AND Price <500

	UNION 

	Select TOP(100)  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description,ROW_NUMBER() OVER (Order By P.Id)+130 as Position
	From Product as P Left JOIN InfoRequest ON P.Id=InfoRequest.ProductId
	Where InfoRequest.Id is NUll
	) as D
	Order by Position
--V2.1 union normal with Filter
	Select *
	From 
	(
	Select top(20) *,1 as Filter
	From Product
	Order By Id Desc

	UNION 

	Select  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description,2 as Filter
	From(
	Select top (100) Product.Id,Product.Name as NameProduct,COUNT(*) as NumRequests
	From Product inner Join InfoRequest On Product.Id=InfoRequest.ProductId
	Group By Product.Id,Product.Name
	order by COUNT(*)
	) AS T INNER JOIN Product as P ON T.Id=P.Id


	UNION 

	Select top(10) *,3 as Filter
	From Product
	Where Price>200 AND Price <500

	UNION 

	Select TOP(100)  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description,4 as Filter
	From Product as P Left JOIN InfoRequest ON P.Id=InfoRequest.ProductId
	Where InfoRequest.Id is NUll
	
	) as D
	Order by Filter
--union all
	Select *
	From (
	Select top(20) *
	From Product
	Order By Id Desc
	) AS T1

	UNION ALL

	Select *
	From
	(
	Select  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description
	From(
	Select top (100) Product.Id,Product.Name as NameProduct
	From Product inner Join InfoRequest On Product.Id=InfoRequest.ProductId
	Group By Product.Id,Product.Name
	order by COUNT(*)
	) AS T INNER JOIN Product as P ON T.Id=P.Id

	)AS T2

	UNION ALL

	Select *
	From
	(
	Select top(10) *
	From Product
	Where Price>200 AND Price <500
	order by Id
	) AS T3

	UNION ALL

	Select *
	From 
	(
	Select TOP(100)  P.Id,P.BrandId,P.Name,P.ShortDescription,P.Price,P.Description
	From Product as P Left JOIN InfoRequest ON P.Id=InfoRequest.ProductId
	Where InfoRequest.Id is NUll
	)AS T4
GO



