using Domain;
using Domain.APIModels;
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
                IdProducts = brand.Products.AsQueryable().MapProductForPBrandPage(),
                Name = brand.BrandName,
                Description = brand.Description,
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
