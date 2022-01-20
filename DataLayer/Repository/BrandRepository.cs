using DataLayer.Interfaces;
using Domain;
using Domain.APIModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repository
{
    public class BrandRepository:Repository<Brand>,IBrandRepository
    {
        public BrandRepository(MyContext context) : base(context) { }

        public int GetCount()
        {
            return _ctx.Brands.Count();
        }

        public List<Brand> GetPageBrands(int page, int pageSize)
        {
            return _ctx.Brands.Include(x => x.Products).Skip(pageSize * (page - 1)).Take(pageSize).Select(brand => new Brand
            {
                Products=brand.Products.Select(product =>new Product { 
                    Id = product.Id,
                
                }).ToList(),
                BrandName=brand.BrandName,
                Description=brand.Description,
            })
            .ToList();
        }
        public BrandDetail GetBrandDetail(int id)
        {
            BrandDetail brandDetail = new BrandDetail();

            var x=GetById(id);
            brandDetail.Id = x.Id;
            brandDetail.Name = x.BrandName;
            brandDetail.TotProducts = GetNumProducts(id);
            brandDetail.CountRequestFromBrandProducts= GetCountInfoRequestsOfAllproducts(id);


            var query = from p in _ctx.Set<Product>()
                        join pc in _ctx.Set<Product_Category>() on p.Id equals pc.ProductId
                        join c in _ctx.Set<Category>() on pc.CategoryId equals c.Id
                        where p.BrandId == id
                        group p by c.Id
            into g
                        select new { g.Key, Count = g.Count() };

            brandDetail.AssociatedCategory = query.Select(cat => new CategoryBrandDetail
            {
                Id = cat.Key,
                Name = _ctx.Categories.SingleOrDefault(x => x.Id == cat.Key).Name,
                CountProdAssociatied=cat.Count,
            }).ToList();

            var query2 = from p in _ctx.Set<Product>()
                        join ir in _ctx.Set<InfoRequest>() on p.Id equals ir.ProductId
                        where p.BrandId == id
                        group p by p.Id
            into g
                        select new { g.Key, Count = g.Count() };
            brandDetail.Products = query2.Select(p => new ProductBrandDetail
            {
                Id=p.Key,
                CountInfoRequest=p.Count,
                Name=_ctx.Products.SingleOrDefault(x=>x.Id == p.Key).Name,
            }).ToList();




            return brandDetail;
        }

        public int GetNumProducts(int id)=> _ctx.Brands.Where(x => x.Id == id).Count();
        public int GetCountInfoRequestsOfAllproducts(int id)=>_ctx.Brands
                                                                .Include(x=>x.Products)
                                                                    .ThenInclude(x=>x.InfoRequests)
                                                                .Count();
        

        
    }
}
