using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using ServicaLayer.ProductService;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using static TestJuniorEFAPI.Controllers.test;

namespace TestJuniorEFAPI.Controllers
{
    

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        ILogger<Product> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ProductService _productService;
        public ProductController(ILogger<Product> logger, IProductRepository productRepository, ProductService productService)
        {
            _productRepository = productRepository;
            _logger = logger;
            _productService = productService;
        }
        /// <summary>
        /// product page 
        /// </summary>
        /// <param name="page">option,default 1,page needed</param>
        /// <param name="pageSize">optional,default 10,products per page</param>
        /// <returns>
        /// NotFound() in case of not valid page number
        /// BadRequest() in case of a not valid pageSize
        /// Ok() return data correctly
        /// </returns>
        /// 
        [Route("page/{page:int}/{pagesize:int}/{brandid:int=0}/{orderby:int=0}/{isasc:bool=false}")]
        public IActionResult GetProductPageAsync(int page, int pageSize,int brandId,int orderBy,bool isAsc)
        {
            if (page <= 0)
                return NotFound("page not found");
            if (pageSize <= 0 || pageSize>1000)
                return BadRequest("page size can't be lower or equal than 0 or higher than 1000");
            if(brandId < 0)
                return BadRequest("there are no brand with id lower than 0");


            return Ok( _productService.GetProductsForPage(page,pageSize,brandId,orderBy,isAsc));
        }
        /// <summary>
        /// product detail page
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns>
        /// BadRequest in case of a not valid id
        /// </returns>
        [Route("Detail/{id}")]
        public IActionResult ProductDetailAsync(int id)
        {
            if (id <= 0)
                return BadRequest("not vaid id");
            return Ok(_productService.GetProductDetail(id));
        }
       














































        [Route("test")]
        public IActionResult testAsync()
        {
            int page = 1;
            int pageSize = 10;
            var query =  new
            {
                PageSize = pageSize,
                Page = page,
                TotalProducts = _productRepository.GetAll().Count(),
                Products = _productRepository.GetAll().OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).MapProductsForPage(),
            };
            var productPageModel = query;

            //var productPageModel = query.ToList();
            return Ok(query);
        }
    }
    public static class test
    {
        public static IQueryable<ProductForPage2> MapProductsForPage(this IQueryable<Product> products)
        {
            return products.Select(product => new ProductForPage2
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.GetFakeImage(),
                ShortDescription = product.ShortDescription,
                
            });
        }
        public class ProductPageModel2
        {
            /// <summary>
            /// page needed
            /// </summary>
            public int Page { get; set; }
            /// <summary>
            /// number of products for each page
            /// </summary>
            public int PageSize { get; set; }
            /// <summary>
            /// total number of products
            /// </summary>
            public int TotalProducts { get; set; }
            public int TotalPages { get; set; }
            /// <summary>
            /// list of products for the page
            /// </summary>
            public IQueryable<ProductForPage2> Products { get; set; }
        }
        /// <summary>
        /// data needed for each product for the product paging api
        /// </summary>
        public class ProductForPage2
        {
            public int Id { get; set; }
            /// <summary>
            /// product name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// product image link
            /// </summary>
            public string Image { get; set; }
            /// <summary>
            /// product short description
            /// </summary>
            public string ShortDescription { get; set; }
        }
    }
   
    
}
