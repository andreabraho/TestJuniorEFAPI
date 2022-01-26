using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Domain.APIModels;
using ServicaLayer.ProductService;

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
        [Route("page/{page:int=1}/{pagesize:int=10}")]
        public IActionResult GetProductPage(int page, int pageSize)
        {
            if (page <= 0)
                return NotFound("page not found");
            if (pageSize <= 0 || pageSize>1000)
                return BadRequest("page size can't be lower or equal than 0 or higher than 1000");
            
            return Ok(_productService.GetProductsForPage(page,pageSize));
        }
        /// <summary>
        /// product detail page
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns>
        /// BadRequest in case of a not valid id
        /// </returns>
        [Route("Detail/{id}")]
        public IActionResult ProductDetail(int id)
        {
            if (id <= 0)
                return BadRequest("not vaid id");
            return Ok(_productService.GetProductDetail(id));
        }
    }
}
