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
        public IActionResult GetProductPae(int page,int pageSize)
        {
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
        [Route("Detail/{id}")]
        public IActionResult ProductDetail(int id)
        {

            return Ok(_productRepository.GetProductDetail(id));
        }
    }
}
