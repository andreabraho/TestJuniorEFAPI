using DataLayer.Interfaces;
using Domain;
using Domain.APIModels;
using ServicaLayer.ProductService.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.ProductService
{
    public class ProductService
    {
        private readonly IRepository<Product> _productRepository;
        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository=productRepository;
        }
        /// <summary>
        /// get all info needed for a brand list page
        /// </summary>
        /// <param name="page">optional,default 1,type int,rappresent the page needed</param>
        /// <param name="pageSize">optional,default 10,type int,rappresents page dimension</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ProductPageModel GetProductsForPage(int page,int pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            var query = _productRepository.GetAll().Select(x=> new ProductPageModel
            {
                PageSize = pageSize,
                Page = page,
                TotalProducts = _productRepository.GetAll().Count(),
                Products = _productRepository.GetAll().OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).Select(product => new ProductForPage
                {
                    Id = product.Id,
                    Name = product.Name,
                    Image = product.GetFakeImage(),
                    ShortDescription = product.ShortDescription,
                }),
            });
            var productPageModel = query.FirstOrDefault();
            productPageModel.TotalPages = CalculateTotalPages(productPageModel.TotalProducts, pageSize);

            return productPageModel; 
        }
        public ProductPageModel GetProductsForPage2(int page,int pageSize)//test
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            var query = _productRepository.GetAll().Select(x=> new ProductPageModel
            {
                PageSize = pageSize,
                Page = page,
                TotalProducts = _productRepository.GetAll().Count(),
                Products = _productRepository.GetAll().OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).MapProductsForPage(),
            });
            var productPageModel = query.FirstOrDefault();
            productPageModel.TotalPages = CalculateTotalPages(productPageModel.TotalProducts, pageSize);

            return productPageModel; 
        }
        /// <summary>
        /// get all detail need for a product detail page
        /// </summary>
        /// <param name="id">rappresents the id of the product witch detail are needed</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        
        public ProductDetailModel GetProductDetail(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var query = _productRepository.GetById(id).Select(p => new ProductDetailModel
            {
                Id = p.Id,
                Name = p.Name,
                BrandName = p.Brand.BrandName,
                productsCategory = p.ProductCategories.Select(c => new CategoryProductModel
                {
                    Id = c.CategoryId,
                    Name = c.Category.Name,
                }),
                countGuestInfoRequests = p.InfoRequests.Where(x => x.UserId == null).Count(),
                countUserInfoRequests = p.InfoRequests.Where(x => x.UserId != null).Count(),
                infoRequestProducts = p.InfoRequests.OrderByDescending(x => x.InsertDate).Select(ir => new InfoRequestProductModel
                {
                    Id = ir.Id,
                    ReplyNumber = ir.InfoRequestReplys.Count(),
                    Name = ir.UserId == null ? ir.Name : ir.User.Name,
                    LastName = ir.UserId == null ? ir.LastName : ir.User.LastName,
                    DateLastReply = ir.InfoRequestReplys.Max(x => x.InsertDate),
                }),
            });
            var x = query.FirstOrDefault(x=>x.Id==id);
            return x;
        }
        public ProductDetailModel GetProductDetail2(int id)//in progress
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var query = _productRepository.GetById(id).MapProductForProductDetail();
            return query.FirstOrDefault();
        }
        private int CalculateTotalPages(int totalItems,int pageSize)
        {
            if(totalItems%pageSize==0)
                return totalItems/pageSize;
            else
                return (totalItems/pageSize)+1;
        }

    }
}
