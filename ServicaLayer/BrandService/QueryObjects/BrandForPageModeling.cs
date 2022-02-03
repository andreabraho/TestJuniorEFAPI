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
        public static IQueryable<BrandForPage> MapBrandForBrandPage(this IQueryable<Brand> brands)
        {
            return brands.Select(brand => new BrandForPage
            {
                Id = brand.Id,
                Name = brand.BrandName,
                Description = brand.Description,
                //IdProducts = brand.Products.AsQueryable().MapProductForPBrandPage(),
            });
        }
        public static IQueryable<ProductForBrandPage> MapProductForPBrandPage(this IQueryable<Product> products)
        {
            return products.Select(p => new ProductForBrandPage
            {
                Id = p.Id,
            });
        }
    }
}
