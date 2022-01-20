using DataLayer.Interfaces;
using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(MyContext context) : base(context) { }

        public int GetCount()
        {
            return _ctx.Products.Count();
        }

        public List<Product> GetPageProducts(int page, int pageSize)
        {
            return _ctx.Products.Skip(pageSize * (page-1)).Take(pageSize).ToList();
        }
        public ProductDetailModel GetProductDetail(int id)
        {
            var x=_ctx.Products.Where(p=>p.Id==id).Select(p=>new ProductDetailModel
            {
                Id = p.Id,
                Name = p.Name,
                BrandName=p.Brand.BrandName,
                productsCategory=p.Product_Categories.Select(c=>new Category
                {
                    Id=c.CategoryId,
                    Name=c.Category.Name,
                    
                }).ToList(),
                countGuestInfoRequests=p.InfoRequests.Where(x=>x.UserId==null).Count(),
                countUserInfoRequests=p.InfoRequests.Where(x=>x.UserId!=null).Count(),
                infoRequestProducts=p.InfoRequests.Select(ir=>new InfoRequestProductModel
                {
                    Id=ir.Id,
                    ReplyNumber=ir.InfoRequestReplys.Count(),
                    Name=ir.UserId==null?ir.Name:ir.User.Name,
                    LastName= ir.UserId == null ? ir.LastName : ir.User.LastName,
                    DateLastReply=ir.InfoRequestReplys.Max(x=>x.InsertDate),
                }).ToList(),
                
            }).FirstOrDefault(x=>x.Id==id);


            return x;
        }
    }
}
