using DataLayer.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(MyContext context) : base(context) { }

        //public int GetCount()
        //{
        //    return _ctx.Brands.Count();
        //}
        ///// <summary>
        ///// get brands for one page
        ///// </summary>
        ///// <param name="page">starts from 1 rappresents the page of Brands needed</param>
        ///// <param name="pageSize">rapprezents the size of each page</param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentOutOfRangeException">in case pagesize is lower or equal than 0</exception>
        ///// <exception cref="ArgumentOutOfRangeException">in case page is lower or equal than 0</exception>
        //public List<Brand> GetPageBrands(int page, int pageSize)
        //{
        //    if (pageSize <= 0)
        //        throw new ArgumentOutOfRangeException(nameof(pageSize));
        //    if (page <= 0)
        //        throw new ArgumentOutOfRangeException(nameof(page));

        //    return _ctx.Brands.Skip(pageSize * (page - 1)).Take(pageSize).Select(brand => new Brand
        //    {
        //        Products = brand.Products.Select(product => new Product
        //        {
        //            Id = product.Id,

        //        }).ToList(),
        //        BrandName = brand.BrandName,
        //        Description = brand.Description,
        //    })
        //    .ToList();
        //}


        
        ///// <summary>
        ///// rappresents neccessary data for a brand detail page
        ///// </summary>
        ///// <param name="id">id of the brand needed</param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentOutOfRangeException">in case of id lower or equal than 0</exception>
        //public BrandDetail GetBrandDetailV2(int id)
        //{

        //    var query = _ctx.Brands.AsQueryable();

        //    var brandDetail =
        //        from Brands in query
        //        let p = Brands.Products
        //        let brandProdCat = Brands.Products.SelectMany(x => x.ProductCategories)
        //        where Brands.Id == id
        //        select new BrandDetail
        //        {
        //            Id = Brands.Id,
        //            Name = Brands.BrandName,
        //            CountRequestFromBrandProducts = p.SelectMany(x => x.InfoRequests).Count(),
        //            TotProducts = Brands.Products.Count(),
        //            Products = p.Select(product => new ProductBrandDetail
        //            {
        //                Id = product.Id,
        //                CountInfoRequest = product.InfoRequests.Count(),
        //                Name = product.Name,

        //            }).ToList(),
        //        };

        //    var t = brandDetail.FirstOrDefault();
        //    #region
        //    /****V1****/
        //    //var query2 = from p in _ctx.Set<Product>()
        //    //            join pc in _ctx.Set<Product_Category>() on p.Id equals pc.ProductId
        //    //            join c in _ctx.Set<Category>() on pc.CategoryId equals c.Id
        //    //            where p.BrandId == id
        //    //            group p by c.Id
        //    //into g
        //    //            select new { g.Key, Count = g.Count() };
        //    //t.AssociatedCategory = query2.Select(cat => new CategoryBrandDetail
        //    //{
        //    //    Id = cat.Key,
        //    //    Name = _ctx.Categories.SingleOrDefault(x => x.Id == cat.Key).Name,
        //    //    CountProdAssociatied = cat.Count,
        //    //}).ToList();
        //    /****V1****/
        //    #endregion
        //    var queryx =
        //            from p in _ctx.Set<Product>()
        //            join pc in _ctx.Set<ProductCategory>() on p.Id equals pc.ProductId
        //            join c in _ctx.Set<Category>() on pc.CategoryId equals c.Id
        //            where p.BrandId == id
        //            group new { c.Id, c.Name } by new { c.Id, c.Name }
        //                into g
        //            select new CategoryBrandDetail
        //            {
        //                Id = g.Key.Id,
        //                Name = g.Key.Name,
        //                CountProdAssociatied = g.Count()
        //            };

        //    var queryz = _ctx.Products_Categories.AsNoTracking()
        //        .Join
        //        (
        //        _ctx.Products,
        //        pc => pc.ProductId,
        //        p => p.Id,
        //        (pc, p) => new { PC = pc, P = p }
        //        )
        //        .Join
        //        (
        //        _ctx.Categories.AsNoTracking(),
        //        pcp => pcp.PC.CategoryId,
        //        c => c.Id,
        //        (pcp, c) => new { PCP = pcp, C = c })
        //        .Where(x => x.PCP.P.BrandId == id)
        //        .GroupBy(g => new { g.C.Id, g.C.Name },
        //        (k, c) => new CategoryBrandDetail
        //        {
        //            Id = Convert.ToInt32(k.Id),
        //            Name = k.Name,
        //            CountProdAssociatied = c.Count()
        //        }
        //        );

        //    t.AssociatedCategory = queryz.ToList();
        //    return t;
        //}
        ///// <summary>
        ///// rappresents neccessary data for a brand detail page
        ///// </summary>
        ///// <param name="id">id of the brand needed</param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentOutOfRangeException">in case of id lower or equal than 0</exception>
        //public IQueryable<BrandDetail> GetBrandDetailV3(int id)
        //{

        //    var query = _ctx.Brands.AsQueryable();

        //    var brandDetail =
        //        from Brands in query
        //        let p = Brands.Products
        //        let brandProdCat = Brands.Products.SelectMany(x => x.ProductCategories)
        //        where Brands.Id == id
        //        select new BrandDetail
        //        {
        //            Id = Brands.Id,
        //            Name = Brands.BrandName,
        //            CountRequestFromBrandProducts = p.SelectMany(x => x.InfoRequests).Count(),
        //            TotProducts = Brands.Products.Count(),
        //            Products = p.Select(product => new ProductBrandDetail
        //            {
        //                Id = product.Id,
        //                CountInfoRequest = product.InfoRequests.Count(),
        //                Name = product.Name,

        //            }).ToList(),
        //            AssociatedCategory = _ctx.Categories.Where(x => brandProdCat.Select(x => x.CategoryId).Contains(x.Id)).Select(ca => new CategoryBrandDetail
        //            {
        //                Id = ca.Id,
        //                Name = ca.Name,
        //                CountProdAssociatied = brandProdCat.Where(d => d.CategoryId == ca.Id).Count(),
        //            }).ToList(),
        //        };

        //    return brandDetail;
        //}



    }
}
