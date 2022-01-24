CREATE TABLE dbo.Commercials(
	StartedAt DATETIME NOT NULL 
	CONSTRAINT PK_Commercials PRIMARY KEY,
	EndedAt DATETIME NOT NULL,
	CommercialName VARCHAR(30) NOT NULL);
GO 
	CREATE TABLE dbo.Calls(CallID INT 
	CONSTRAINT PK_Calls NOT NULL PRIMARY KEY,
	AirTime DATETIME NOT NULL,
	SomeInfo CHAR(300)); 
GO 
CREATE UNIQUE INDEX Calls_AirTime
	ON dbo.Calls(AirTime) INCLUDE(SomeInfo);
GO

  CREATE TABLE dbo.Numbers(n INT NOT NULL PRIMARY KEY) GO DECLARE @i INT; SET @i = 1; INSERT INTO dbo.Numbers(n) SELECT 1;
  WHILE @i<1024000 
  BEGIN
  INSERT INTO dbo.Numbers(n)
    SELECT n + @i FROM dbo.Numbers;
  SET @i = @i * 2; 
  END;
  GO 

  INSERT INTO dbo.Commercials(StartedAt, EndedAt, CommercialName) SELECT DATEADD(minute, n - 1, '20080101')
   ,DATEADD(minute, n, '20080101')
   ,'Show #'+CAST(n AS VARCHAR(6))
  FROM dbo.Numbers
  WHERE n<=24*365*60; 
  GO
  INSERT INTO dbo.Calls(CallID,
  AirTime,
  SomeInfo) SELECT n 
   ,DATEADD(minute, n - 1, '20080101')
   ,'Call during Commercial #'+CAST(n AS VARCHAR(6))
  FROM dbo.Numbers
  WHERE n<=24*365*60; 
  GO 