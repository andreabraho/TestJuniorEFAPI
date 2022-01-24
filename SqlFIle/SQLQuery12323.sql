SELECT [a].[Id], [a].[AccountType], [a].[Email], [a].[Password], [b].[Id], [b].[AccountId], [b].[BrandName], [b].[Description], [p].[Id], [p].[BrandId], [p].[Description], [p].[Name], [p].[Price], [p].[ShortDescription], [p].[UserId]
      FROM [Account] AS [a]
      LEFT JOIN [Brand] AS [b] ON [a].[Id] = [b].[AccountId]
      LEFT JOIN [Product] AS [p] ON [b].[Id] = [p].[BrandId]
      WHERE [a].[Id] = 1
      ORDER BY [a].[Id], [p].[Id]


	  SELECT [a].[Id], [a].[AccountType], [a].[Email], [a].[Password], [b].[Id], [b].[AccountId], [b].[BrandName], [b].[Description], [p].[Id], [p].[BrandId], [p].[Description], [p].[Name], [p].[Price], [p].[ShortDescription], [p].[UserId]
      FROM [Account] AS [a]
      LEFT JOIN [Brand] AS [b] ON [a].[Id] = [b].[AccountId]
      LEFT JOIN [Product] AS [p] ON [b].[Id] = [p].[BrandId]
      WHERE [a].[Id] = 1
      ORDER BY [a].[Id], [p].[Id]




	        SELECT [a].[Id], [a].[AccountType], [a].[Email], [a].[Password], [b].[Id], [b].[AccountId], [b].[BrandName], [b].[Description], [t].[Id], [t].[BrandId], [t].[Description], [t].[Name], [t].[Price], [t].[ShortDescription], [t].[UserId], [t].[Id0], [t].[AccountId], [t].[BrandName], [t].[Description0]
      FROM [Account] AS [a]
      LEFT JOIN [Brand] AS [b] ON [a].[Id] = [b].[AccountId]
      LEFT JOIN (
          SELECT [p].[Id], [p].[BrandId], [p].[Description], [p].[Name], [p].[Price], [p].[ShortDescription], [p].[UserId], [b0].[Id] AS [Id0], [b0].[AccountId], [b0].[BrandName], [b0].[Description] AS [Description0]
          FROM [Product] AS [p]
          INNER JOIN [Brand] AS [b0] ON [p].[BrandId] = [b0].[Id]
      ) AS [t] ON [b].[Id] = [t].[BrandId]
      WHERE [a].[Id] = 1
      ORDER BY [a].[Id], [t].[Id], [t].[Id0]



	  SELECT [a].[Id], [a].[AccountType], [a].[Email], [a].[Password], [b].[Id], [b].[AccountId], [b].[BrandName], [b].[Description], [p].[Id], [p].[BrandId], [p].[Description], [p].[Name], [p].[Price], [p].[ShortDescription]
      FROM [Account] AS [a]
      LEFT JOIN [Brand] AS [b] ON [a].[Id] = [b].[AccountId]
      LEFT JOIN [Product] AS [p] ON [b].[Id] = [p].[BrandId]
      WHERE [a].[Id] = 1
      ORDER BY [a].[Id], [p].[Id]



SELECT [a].[Id], [a].[AccountType], [a].[Email], [a].[Password], [b].[Id], [b].[AccountId], [b].[BrandName], [b].[Description], [t0].[Id], [t0].[BrandId], [t0].[Description], [t0].[Name], [t0].[Price], [t0].[ShortDescription], [t0].[Id0], [t0].[Cap], [t0].[Citta], [t0].[Email], [t0].[InsertDate], [t0].[LastName], [t0].[Name0], [t0].[NationId], [t0].[Telefono], [t0].[ProductId], [t0].[RequestText], [t0].[UserId], [t0].[Id00], [t0].[AccountId], [t0].[InfoRequestId], [t0].[InsertDate0], [t0].[ReplyText]
FROM [Account] AS [a]
LEFT JOIN [Brand] AS [b] ON [a].[Id] = [b].[AccountId]
LEFT JOIN (
SELECT [p].[Id], [p].[BrandId], [p].[Description], [p].[Name], [p].[Price], [p].[ShortDescription], [t].[Id] AS [Id0], [t].[Cap], [t].[Citta], [t].[Email], [t].[InsertDate], [t].[LastName], [t].[Name] AS [Name0], [t].[NationId], [t].[Telefono], [t].[ProductId], [t].[RequestText], [t].[UserId], [t].[Id0] AS [Id00], [t].[AccountId], [t].[InfoRequestId], [t].[InsertDate0], [t].[ReplyText]
FROM [Product] AS [p]
LEFT JOIN (
	SELECT [i].[Id], [i].[Cap], [i].[Citta], [i].[Email], [i].[InsertDate], [i].[LastName], [i].[Name], [i].[NationId], [i].[Telefono], [i].[ProductId], [i].[RequestText], [i].[UserId], [i0].[Id] AS [Id0], [i0].[AccountId], [i0].[InfoRequestId], [i0].[InsertDate] AS [InsertDate0], [i0].[ReplyText]
	FROM [InfoRequest] AS [i]
	LEFT JOIN [InfoRequestReply] AS [i0] ON [i].[Id] = [i0].[InfoRequestId]
) AS [t] ON [p].[Id] = [t].[ProductId]
) AS [t0] ON [b].[Id] = [t0].[BrandId]
WHERE [a].[Id] = 1
ORDER BY [a].[Id], [t0].[Id], [t0].[Id0], [t0].[Id00]



SELECT [p].[Id], [p].[BrandId], [p].[Description], [p].[Name], [p].[Price], [p].[ShortDescription], [t].[CategoryId], [t].[ProductId], [t].[Id], [t].[Name]
FROM [Product] AS [p]
LEFT JOIN (
    SELECT [p0].[CategoryId], [p0].[ProductId], [c].[Id], [c].[Name]
    FROM [Product_Category] AS [p0]
    INNER JOIN [Category] AS [c] ON [p0].[CategoryId] = [c].[Id]
) AS [t] ON [p].[Id] = [t].[ProductId]
ORDER BY [p].[Id], [t].[CategoryId], [t].[ProductId], [t].[Id]



SELECT [t].[BrandName], [t].[Description], [t].[Id], [t0].[Id]
FROM (
SELECT [b].[BrandName], [b].[Description], [b].[Id]
FROM [Brand] AS [b]
ORDER BY (SELECT 1)
OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY
) AS [t]
OUTER APPLY (
SELECT [p].[Id]
FROM [Product] AS [p]
WHERE [t].[Id] = [p].[BrandId]
) AS [t0]



SELECT [t].[BrandName], [t].[Description], [t].[Id], [p].[Id]
FROM (
    SELECT [b].[BrandName], [b].[Description], [b].[Id]
    FROM [Brand] AS [b]
    ORDER BY (SELECT 1)
    OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY
) AS [t]
LEFT JOIN [Product] AS [p] ON [t].[Id] = [p].[BrandId]




Select C.Id,C.Name
From Product as P Join Product_Category as PC ON P.Id=PC.ProductId
		JOIN Category As C ON PC.CategoryId=C.Id
WHERE P.BrandId=1

Select C.Id,C.Name,Count(*) as ProdAsscociatedToCat
From Product as P Join Product_Category as PC ON P.Id=PC.ProductId
		JOIN Category As C ON PC.CategoryId=C.Id
WHERE P.BrandId=1
Group BY C.Id,C.Name



SELECT [c0].[Id], (
    SELECT TOP(1) [c].[Name]
    FROM [Category] AS [c]
    WHERE [c].[Id] = [c0].[Id]) AS [Name], COUNT(*) AS [CountProdAssociatied]
FROM [Product] AS [p]
INNER JOIN [Product_Category] AS [p0] ON [p].[Id] = [p0].[ProductId]
INNER JOIN [Category] AS [c0] ON [p0].[CategoryId] = [c0].[Id]
WHERE [p].[BrandId] = 22
GROUP BY [c0].[Id]

select p.Id,count(*) FROM [Product] AS [p]
INNER JOIN [Product_Category] AS [p0] ON [p].[Id] = [p0].[ProductId]
INNER JOIN [Category] AS [c0] ON [p0].[CategoryId] = [c0].[Id]
WHERE [p].[BrandId] = 22
group by p.Id