using DataLayer.Interfaces;
using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyContext context) : base(context) { }

        public int GetCount()
        {
            return _ctx.Products.Count();
        }
        /// <summary>
        /// get products for a page
        /// </summary>
        /// <param name="page">page wanted</param>
        /// <param name="pageSize">page dimension</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">in case page or pageSize<=0</exception>
        public List<Product> GetPageProducts(int page, int pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            return _ctx.Products.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
        }
        /// <summary>
        /// data for a product detail page
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">if id <=0 </exception>
        public ProductDetailModel GetProductDetail(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var x = _ctx.Products.Where(p => p.Id == id).Select(p => new ProductDetailModel
            {
                Id = p.Id,
                Name = p.Name,
                BrandName = p.Brand.BrandName,
                productsCategory = p.Product_Categories.Select(c => new Category
                {
                    Id = c.CategoryId,
                    Name = c.Category.Name,

                }).ToList(),
                countGuestInfoRequests = p.InfoRequests.Where(x => x.UserId == null).Count(),
                countUserInfoRequests = p.InfoRequests.Where(x => x.UserId != null).Count(),
                infoRequestProducts = p.InfoRequests.OrderByDescending(x => x.InsertDate).Select(ir => new InfoRequestProductModel
                {
                    Id = ir.Id,
                    ReplyNumber = ir.InfoRequestReplys.Count(),
                    Name = ir.UserId == null ? ir.Name : ir.User.Name,
                    LastName = ir.UserId == null ? ir.LastName : ir.User.LastName,
                    DateLastReply = ir.InfoRequestReplys.Max(x => x.InsertDate),
                }).ToList(),

            }).FirstOrDefault();


            return x;
        }
        /// <summary>
        /// data for a product detail page
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">if id <=0 </exception>
        public ProductDetailModel GetProductDetailV2(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var query = _ctx.Products.AsQueryable();

            var t =
                from Products in query
                let brand = Products.Brand
                let ir = Products.InfoRequests
                let pr = Products.Product_Categories
                where Products.Id == id
                select new ProductDetailModel
                {
                    BrandName = brand.BrandName,
                    Id = Products.Id,
                    Name = Products.Name,
                    countGuestInfoRequests = ir.Where(x => x.UserId == null).Count(),
                    countUserInfoRequests = ir.Where(x => x.UserId != null).Count(),
                    productsCategory = pr.Select(c => new Category
                    {
                        Id = c.CategoryId,
                        Name = c.Category.Name,

                    }).ToList(),
                    infoRequestProducts = ir.OrderByDescending(x => x.InsertDate).Select(ir => new InfoRequestProductModel
                    {
                        Id = ir.Id,
                        ReplyNumber = ir.InfoRequestReplys.Count(),
                        Name = ir.UserId == null ? ir.Name : ir.User.Name,
                        LastName = ir.UserId == null ? ir.LastName : ir.User.LastName,
                        DateLastReply = ir.InfoRequestReplys.Max(x => x.InsertDate),
                    }).ToList()

                };
            return t.FirstOrDefault();
        }
    }
}
