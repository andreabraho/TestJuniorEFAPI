
ALTER INDEX IX_InsertDates_InsertDate  ON InfoRequestReply  
DISABLE;  
ALTER INDEX IX_AccountTypes_AccountType  ON InfoRequestReply  
DISABLE;
ALTER INDEX IX_InserDates_InsertDate  ON InfoRequestReply  
DISABLE; 
ALTER INDEX IX_InfoRequestId_ProductId  ON InfoRequest  
DISABLE; 
ALTER INDEX IX_Product_BrandIdPrice  ON Product 
DISABLE; 

ALTER INDEX IX_InsertDates_InsertDate  ON InfoRequestReply  
REBUILD;  
ALTER INDEX IX_AccountTypes_AccountType  ON InfoRequestReply  
REBUILD;
ALTER INDEX IX_InserDates_InsertDate  ON InfoRequestReply  
REBUILD; 
ALTER INDEX IX_InfoRequestId_ProductId  ON InfoRequest  
REBUILD; 
ALTER INDEX IX_Product_BrandIdPrice  ON Product 
REBUILD; 