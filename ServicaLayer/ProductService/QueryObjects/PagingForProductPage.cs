using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.ProductService.QueryObjects
{
    public static class PagingForProductPage
    {
        public static IQueryable<Product> Paging(this IQueryable<Product> products,int page,int pageSize)
        {
            products=products.Skip(pageSize * (page - 1)).Take(pageSize);
            return products;    
        }
    }
}
