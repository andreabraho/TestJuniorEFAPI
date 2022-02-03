using Domain;
using ServicaLayer.ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.ProductService.QueryObjects
{
    public static class ProductForPageModeling
    {

        public static IQueryable<ProductForPage> MapProductsForPage(this IQueryable<Product> products)
        {
            return products.Select(product => new ProductForPage
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.GetFakeImage(),
                ShortDescription = product.ShortDescription,
                BrandId = product.BrandId,
                BrandName = product.Brand.BrandName,
                Categories = product.ProductCategories.Select(c => c.Category).ToList(),
                Price=product.Price,
            });


        }
    }
}

