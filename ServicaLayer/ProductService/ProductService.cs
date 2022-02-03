using DataLayer.Interfaces;
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
        /// <param name="brandId">optional,default 0,type int,if default does nothing , if different filters on brands</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ProductPageModel GetProductsForPage(int page,int pageSize,int brandId=0,int orderBy=1,bool isAsc=true)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if(brandId < 0)
                throw new ArgumentOutOfRangeException(nameof(brandId));


            var productPageModel =  new ProductPageModel
            {
                PageSize = pageSize,
                Page = page,
                TotalProducts = _productRepository.GetAll().Count(),
                
            };
            var query = _productRepository.GetAll();

            if(brandId > 0)
            {
                query = query.Where(x => x.BrandId == brandId);
            }

            switch (orderBy)
            {
                case 1:
                    if(isAsc)
                        query=query.OrderBy(x=>x.Brand.BrandName);
                    else
                        query = query.OrderByDescending(x => x.Brand.BrandName);
                    break;
                case 2:
                    if (isAsc)
                        query = query.OrderBy(x => x.Name);
                    else
                        query = query.OrderByDescending(x => x.Name);
                    break ;
                case 3:
                    if(isAsc)
                        query=query.OrderBy(x=>x.Price);
                    else
                        query = query.OrderByDescending(x => x.Price);
                    break ;
                default:
                    query = query.OrderBy(x => x.Brand.BrandName).ThenBy(x => x.Name);
                    break;
            }

            query = query.Skip(pageSize * (page - 1)).Take(pageSize);


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




















        //test not working-----------------------------------------------------------------------------------------------

        public async Task<ProductPageModel> GetProductsForPage2(int page, int pageSize)//in progress
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            var query = _productRepository.GetAll().Select(x => new ProductPageModel
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
