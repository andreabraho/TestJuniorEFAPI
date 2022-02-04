using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using Microsoft.EntityFrameworkCore;
using ServicaLayer.ProductService.Model;
using ServicaLayer.ProductService.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicaLayer.ProductService
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository=productRepository;
        }
        /// <summary>
        /// get all info needed for a brand list page
        /// </summary>
        /// <param name="page">optional,default 1,type int,rappresent the page needed</param>
        /// <param name="pageSize">optional,default 10,type int,rappresents page dimension</param>
        /// <param name="brandId">optional,default 0,type int,if default does nothing , if different filters on brands</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ProductPageDTO GetProductsForPage(int page,int pageSize,int brandId=0,int orderBy=1,bool isAsc=true)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if(brandId < 0)
                throw new ArgumentOutOfRangeException(nameof(brandId));

            var productPageModel =  new ProductPageDTO
            {
                PageSize = pageSize,
                Page = page,
                TotalProducts = _productRepository.GetAll().Count(),
                
            };
            var query = _productRepository.GetAll();

            query = query.FilterProducts(brandId);

            query=query.OrderForPage(orderBy,isAsc);

            query = query.Page(page, pageSize);

            productPageModel.Products = query.MapProductsForPage();

            productPageModel.TotalPages = CalculateTotalPages(productPageModel.TotalProducts, pageSize);

            return productPageModel; 
        }
        
        /// <summary>
        /// get all detail need for a product detail page
        /// </summary>
        /// <param name="id">rappresents the id of the product witch detail are needed</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        
        async public Task<ProductDetailDTO> GetProductDetail(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var query = _productRepository.GetById(id).Select(p => new ProductDetailDTO
            {
                Id = p.Id,
                Name = p.Name,
                BrandName = p.Brand.BrandName,
                productsCategory = p.ProductCategories.Select(c => new CategoryProductDTO
                {
                    Id = c.CategoryId,
                    Name = c.Category.Name,
                }),
                countGuestInfoRequests = p.InfoRequests.Where(x => x.UserId == null).Count(),
                countUserInfoRequests = p.InfoRequests.Where(x => x.UserId != null).Count(),
                infoRequestProducts = p.InfoRequests.OrderByDescending(x => x.InsertDate).Select(ir => new InfoRequestProductDTO
                {
                    Id = ir.Id,
                    ReplyNumber = ir.InfoRequestReplys.Count(),
                    Name = ir.UserId == null ? ir.Name : ir.User.Name,
                    LastName = ir.UserId == null ? ir.LastName : ir.User.LastName,
                    DateLastReply = ir.InfoRequestReplys.Max(x => x.InsertDate),
                }),
            });
            var productDetail = await query.FirstOrDefaultAsync();
            return productDetail;
        }


        /// <summary>
        /// insert a product and categories associated to him
        /// </summary>
        /// <param name="product">product</param>
        /// <param name="categories">list of in rappresenting the categories associated</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">null input</exception>
        public async Task<bool> AddProduct(Product product,int[] categories)
        {
            if(product == null)
                throw new ArgumentNullException(nameof(product));
            if(categories == null) 
                throw new ArgumentNullException(nameof(categories));
            
            return await _productRepository.InsertWithCat(product, categories);

        }
        /// <summary>
        /// delete a product and all data related
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"> id not valid</exception>
        public async Task<bool> DeleteProduct(int id)
        {
            if(id<=0)
                throw new ArgumentException(nameof(id));

            return await _productRepository.DeleteAll(id);
        }






















        //test not working-----------------------------------------------------------------------------------------------

        public async Task<ProductPageDTO> GetProductsForPage2(int page, int pageSize)//in progress
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            var query = _productRepository.GetAll().Select(x => new ProductPageDTO
            {
                PageSize = pageSize,
                Page = page,
                TotalProducts = _productRepository.GetAll().Count(),
                Products = _productRepository.GetAll().OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).MapProductsForPage()
            });
            var productPageModel = await query.FirstOrDefaultAsync();
            productPageModel.TotalPages = CalculateTotalPages(productPageModel.TotalProducts, pageSize);

            return productPageModel;
        }
        public ProductDetailDTO GetProductDetail2(int id)//in progress
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
