update InfoRequestReply
Set InfoRequestReply.IsDeleted=0
From InfoRequestReply as irr join InfoRequest as ir On irr.InfoRequestId=ir.Id join Product as p On ir.ProductId=p.Id
where p.BrandId=50

update InfoRequest
Set InfoRequest.IsDeleted=0
From InfoRequest as ir join Product as p On ir.ProductId=p.Id
where p.BrandId=50


update Product
Set IsDeleted=0
where BrandId=50




update Product_Category
set IsDeleted=1
where ProductId=1



update Account
Set IsDeleted=0
update Brand
Set IsDeleted=0
update Category
Set IsDeleted=0
update InfoRequest
Set IsDeleted=0
update InfoRequestReply
Set IsDeleted=0
update Nation
Set IsDeleted=0
update Product_Category
Set IsDeleted=0
update Product
Set IsDeleted=0
update [User]
Set IsDeleted=0