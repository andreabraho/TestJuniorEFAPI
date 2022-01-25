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
        public IActionResult GetBrandPage(int page, int pageSize)
        {
            if (page <= 0)
                return NotFound("page not found");
            if (pageSize <= 0)
                return BadRequest("page size can't be lower or equal than 0");
            BrandPageModel brandPageModel = new BrandPageModel();
            brandPageModel.PageSize = pageSize;
            brandPageModel.Page = page;
            brandPageModel.TotalBrand = _brandRepository.GetCount();

            return Ok(brandPageModel);
        }
        /// <summary>
        /// brand detail page
        /// </summary>
        /// <param name="id">id of the brand</param>
        /// <returns>
        /// BadRequest() in case of a not valid id
        /// Ok () return data correctly
        /// </returns>
        [Route("Detail/{id}")]
        public IActionResult GetBrandDetail(int id)
        {
            if (id <= 0)
                return BadRequest("id not valid");

            return Ok(_brandRepository.GetBrandDetailV3(id).FirstOrDefault());
        }

    }
}
