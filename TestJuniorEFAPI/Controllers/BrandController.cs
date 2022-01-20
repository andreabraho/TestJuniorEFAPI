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
    public class BrandController : ControllerBase
    {
        private readonly ILogger<Brand> _logger;
        private readonly IBrandRepository _brandRepository;
        public BrandController(ILogger<Brand> logger, IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }

        [Route("Page/{page}/{pageSize}")]
        public IActionResult GetProductPae(int page, int pageSize)
        {
            
            BrandPageModel brandPageModel = new BrandPageModel();
            brandPageModel.PageSize = pageSize;
            brandPageModel.Page = page;
            brandPageModel.TotalBrand = _brandRepository.GetCount();
            brandPageModel.Brands = _brandRepository.GetPageBrands(page, pageSize).Select(brand => new BrandForPage
            {
                Name = brand.BrandName,
                Description = brand.Description,
                IdProducts=brand.Products.Select(product => product.Id).ToList()

            }).ToList();

            return Ok(brandPageModel);
        }
        [Route("Detail/{id}")]
        public IActionResult GetBrandDetail(int id)
        {

            /*
             Select C.Id,C.Name,Count(*) as ProdAsscociatedToCat
            From Product as P Join Product_Category as PC ON P.Id=PC.ProductId
		            JOIN Category As C ON PC.CategoryId=C.Id
            WHERE P.BrandId=1
            Group BY C.Id,C.Name 
            */

            return Ok(_brandRepository.GetBrandDetail(id));
        }

    }
}
