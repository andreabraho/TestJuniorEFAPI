using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using Domain.ModelsForApi;
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
        private readonly IBrandRepository _brandRepository;
        private readonly IRepository<Category> _categoryRepository;
        public ProductService(IProductRepository productRepository,IBrandRepository brandRepository, IRepository<Category> categoryRepository)
        {
            _productRepository=productRepository;
            _brandRepository=brandRepository;
            _categoryRepository=categoryRepository;
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

            var productPageModel = new ProductPageDTO
            {
                PageSize = pageSize,
                Page = page,
                TotalProducts = _productRepository.GetAll().FilterProducts(brandId).Count(),
                Brands = _brandRepository.GetAll().Select(b => new BrandForPageDTO
                {
                    Id = b.Id,
                    Name = b.BrandName
                })
                
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
                BrandId=p.Brand.Id,
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
        /// upsert a product 
        /// </summary>
        /// <param name="product">product data to be upserted</param>
        /// <param name="categories"> categories of the product</param>
        /// <returns>0 in case of error,n in case of success</returns>
        /// <exception cref="ArgumentNullException">product null</exception>
        /// <exception cref="ArgumentNullException">categories null</exception>
        public async Task<int> UpsertProduct(Product product,int[] categories)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (categories == null)
                throw new ArgumentNullException(nameof(categories));

            return await _productRepository.Upsert(product,categories);
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

            return await _productRepository.DeleteProdAndRelatedData(id);
        }
        
        /// <summary>
        /// get all data needed for product upsert page
        /// </summary>
        /// <returns></returns>
        public  GetInsertProductDTO GetInsertProductDTO()
        {
            var x=new GetInsertProductDTO();
            x.Brands = _brandRepository.GetAll().Select(brand => new BrandForInsertDTO
            {
                Id = brand.Id,
                Name = brand.BrandName

            });
            x.Categories = _categoryRepository.GetAll().Select(cat => new CatForInsertDTO
            {
                Id = cat.Id,
                Name = cat.Name,
            });
            return x;
        }
        /// <summary>
        /// get a product data needed for update
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null or the product needed</returns>
        public GetUpdateProductDTO GetProductForUpdate(int id)
        {
            if(id<=0)
                throw new ArgumentException(nameof(id));

            var getUpdateProductDTO = _productRepository.GetById(id).Include(x => x.ProductCategories).Select(x=>new GetUpdateProductDTO
            {
                AllBrands=_brandRepository.GetAll().Select(brand=>new BrandForInsertDTO
                {
                    Id=brand.Id,
                    Name=brand.BrandName
                }),
                AllCategories=_categoryRepository.GetAll().Select(cat=>new CatForInsertDTO
                {
                    Name=cat.Name,
                    Id=cat.Id,
                }),
                CategoriesAssociated=x.ProductCategories.Select(cat=>cat.CategoryId).ToArray(),
                Product= _productRepository.GetById(id).FirstOrDefault()

            }).FirstOrDefault();

            return getUpdateProductDTO;

        }
        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }
        /// <summary>
        /// validates if the model for product upsert is valid
        /// </summary>
        /// <param name="prodWCat"></param>
        /// <returns>null is it is valid,string with error if not</returns>
        private string UpSertProductValidation(ProdWithCat prodWCat)
        {
            string result = null;

            if (prodWCat.CategoriesIds.Length == 0)
            {
                result += "Select at least one category for the product \n";
            }
            if (prodWCat.Product.Name.Length == 0 || prodWCat.Product.Name.Length > 255 ||string.IsNullOrWhiteSpace(prodWCat.Product.Name))
            {
                result += "Product name can't be empity and can't have more than 255 characters \n";
            }
            if (prodWCat.Product.ShortDescription.Length == 0 || prodWCat.Product.ShortDescription.Length > 255|| string.IsNullOrWhiteSpace(prodWCat.Product.ShortDescription))
            {
                result += "Product short description can't be empity and can't have more than 255 characters \n";
            }
            if (prodWCat.Product.Price < 0 || prodWCat.Product.Price > (decimal)1e16)
            {
                result += "Price can't be lower than 0 or higher than 1e16";
            }
            if (prodWCat.Product.BrandId == 0)
            {
                result += "Brand id can't be 0";
            }
            return result;
        }


    }
    
}
