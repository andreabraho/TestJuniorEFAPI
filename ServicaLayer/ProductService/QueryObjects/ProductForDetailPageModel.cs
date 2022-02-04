using Domain;
using ServicaLayer.ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.ProductService.QueryObjects
{
    public static class ProductForDetailPageModel
    {
        public static IQueryable<ProductDetailDTO> MapProductForProductDetail(this IQueryable<Product> product)
        {
            return product.Select(p => new ProductDetailDTO
            {
                Id = p.Id,
                Name = p.Name,
                BrandName = p.Brand.BrandName,
                productsCategory = p.ProductCategories.AsQueryable().MapCategoryForProductDetail(),//error generation
                countGuestInfoRequests = p.InfoRequests.Where(x => x.UserId == null).Count(),
                countUserInfoRequests = p.InfoRequests.Where(x => x.UserId != null).Count(),
                infoRequestProducts = p.InfoRequests.AsQueryable().OrderByDescending(x => x.InsertDate).MapIRForProductDetail(),//error generation
            });
        }
        public static IQueryable<CategoryProductDTO> MapCategoryForProductDetail(this IQueryable<ProductCategory> categories)
        {
            return categories.Select(c => new CategoryProductDTO
            {
                Id = c.CategoryId,
                Name = c.Category.Name,
            });
        }
        public static IQueryable<InfoRequestProductDTO> MapIRForProductDetail (this IQueryable<InfoRequest> infoRequests)
        {
            return infoRequests.Select(ir => new InfoRequestProductDTO
            {
                Id = ir.Id,
                ReplyNumber = ir.InfoRequestReplys.Count(),
                Name = ir.UserId == null ? ir.Name : ir.User.Name,
                LastName = ir.UserId == null ? ir.LastName : ir.User.LastName,
                DateLastReply = ir.InfoRequestReplys.Max(x => x.InsertDate),
            });
        }
    }
}
