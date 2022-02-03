using Domain;
using ServicaLayer.InfoRequestService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.InfoRequestService.QueryObjects
{
    public static class MapInfoRequestPageModel
    {
        public static IQueryable<IRForPageModel> MapIrForPaging(this IQueryable<InfoRequest> infoRequest)
        {
            return infoRequest.Select(ir => new IRForPageModel
            {
                Id = ir.Id,
                User = new UserModelForIR
                {
                    Name = ir.Name,
                    LastName = ir.LastName,
                    PhoneNumber = ir.PhoneNumber,
                    Email = ir.Email,
                    City = ir.City,
                    Cap = ir.Cap,
                },
                RequestText = ir.RequestText,
                InsertDate = ir.InsertDate,
                ProductId = ir.ProductId,
                ProductName = ir.Product.Name,
                BrandId = ir.Product.BrandId,
                BrandName = ir.Product.Brand.BrandName
            });
        }
    }
}
