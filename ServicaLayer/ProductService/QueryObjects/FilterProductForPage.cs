using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.ProductService.QueryObjects
{
    public static class FilterProductForPage
    {
        public static IQueryable<Product> FilterProducts(this IQueryable<Product> products,int brandId=0)
        {
            if (brandId > 0)
            {
                products = products.Where(x => x.BrandId == brandId);
            }
            return products;    
        }
    }
}
