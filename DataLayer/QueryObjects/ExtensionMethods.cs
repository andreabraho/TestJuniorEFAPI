using Domain;
using Domain.ModelsForApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.QueryObjects
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Product> MapToProducts(this ProdWithCat[] products)
        {
            return products.Select(p => new Product
            {
                Name = p.Product.Name,
                ShortDescription = p.Product.ShortDescription,
                Description = p.Product.Description,
                Price = p.Product.Price,
                ProductCategories = p.CategoriesIds.Select(c => new ProductCategory
                {
                    CategoryId = c,
                }).ToList(),
            }).ToList();
        }
    }
}
