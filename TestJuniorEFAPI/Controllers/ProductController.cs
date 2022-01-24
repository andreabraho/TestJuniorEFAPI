using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Domain.APIModels;

namespace TestJuniorEFAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        ILogger<Product> _logger;
        private IProductRepository _productRepository;
        public ProductController(ILogger<Product> logger,IProductRepository productRepository)
        { 
            this._productRepository = productRepository;
            this._logger = logger;
        }

        [Route("Page/{page}/{pageSize}")]
        public IActionResult GetProductPage(int page,int pageSize)
        {
            if(page<=0)
                return NotFound("page not found");
            if(pageSize <= 0)
                return BadRequest("page size can't be lower or equal than 0");
            ProductPageModel  productPageModel = new ProductPageModel();
            productPageModel.PageSize = pageSize;
            productPageModel.Page = page;
            productPageModel.TotalProducts=_productRepository.GetCount();
            productPageModel.Products = _productRepository.GetPageProducts(page, pageSize).Select(product=>new ProductForPage
            {
                Id = product.Id,
                Name = product.Name,
                Image=product.GetFakeImage(),
                ShortDescription=product.ShortDescription,
            }).ToList();
            return Ok(productPageModel);
        }
        /// <summary>
        /// product detail page
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns></returns>
        [Route("Detail/{id}")]
        public IActionResult ProductDetail(int id)
        {
            if (id <= 0)
                return BadRequest("not vaid id");
            return Ok(_productRepository.GetProductDetail(id));
        }
    }
}
