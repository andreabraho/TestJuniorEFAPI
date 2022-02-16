using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.ProductService.QueryObjects
{
    public static class OrderProductsForPage
    {
        public static IQueryable<Product> OrderForPage(this IQueryable<Product> products,OrderProduct orderBy,bool isAsc)
        {
            switch (orderBy)
            {
                case OrderProduct.BrandName:
                    if (isAsc)
                        products = products.OrderBy(x => x.Brand.BrandName);
                    else
                        products = products.OrderByDescending(x => x.Brand.BrandName);
                    break;
                case OrderProduct.ProductName:
                    if (isAsc)
                        products = products.OrderBy(x => x.Name);
                    else
                        products = products.OrderByDescending(x => x.Name);
                    break;
                case OrderProduct.Price:
                    if (isAsc)
                        products = products.OrderBy(x => x.Price);
                    else
                        products = products.OrderByDescending(x => x.Price);
                    break;
                default:
                    products = products.OrderBy(x => x.Brand.BrandName).ThenBy(x => x.Name);
                    break;
            }
            return products;
        }
    }
}
