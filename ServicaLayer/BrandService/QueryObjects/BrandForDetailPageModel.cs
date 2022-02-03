﻿using Domain;
using ServicaLayer.BrandService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.BrandService.QueryObjects
{
    public static class BrandForDetailPageModel
    {
        public static IQueryable<ProductBrandDetail> MapProductsForBrandDetail(this IQueryable<Product> products)
        {
            return products.Select(product => new ProductBrandDetail
            {
                Id = product.Id,
                CountInfoRequest = product.InfoRequests.Count(),
                Name = product.Name,

            });
        }
    }
}
