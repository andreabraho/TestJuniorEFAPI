using Domain;
using ServicaLayer.BrandService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.BrandService.QueryObjects
{
    public static class BrandForPageModeling
    {
        public static IQueryable<BrandForPageDTO> MapBrandForBrandPage(this IQueryable<Brand> brands)
        {
            return brands.Select(brand => new BrandForPageDTO
            {
                Id = brand.Id,
                Name = brand.BrandName,
                Description = brand.Description,
                //IdProducts = brand.Products.AsQueryable().MapProductForPBrandPage(),
            });
        }
        //public static IQueryable<ProductForBrandPageDTO> MapProductForPBrandPage(this IQueryable<Product> products)
        //{
        //    return products.Select(p => new ProductForBrandPageDTO
        //    {
        //        Id = p.Id,
        //    });
        //}
    }
}
