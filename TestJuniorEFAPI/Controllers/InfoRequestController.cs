using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicaLayer.InfoRequestService;
using System.Linq;
using System.Threading.Tasks;

namespace TestJuniorEFAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoRequestController : ControllerBase
    {
        private readonly ILogger<InfoRequest> _logger;
        private readonly InfoRequestService _infoRequestService;
        public InfoRequestController(ILogger<InfoRequest> logger, InfoRequestService infoRequestService)
        {
            _infoRequestService = infoRequestService;
            _logger = logger;
        }
        [Route("Page/{page:int=1}/{pageSize:int=10}")]
        public IActionResult GetPage(int page,int pageSize,int brandId,string prodNameSearch,bool isAsc,int productId)
        {
            if (page <= 0)
                return NotFound("page not found");
            if (pageSize <= 0 || pageSize > 1000)
                return BadRequest("page size can't be lower or equal than 0 or higher than 1000");
            if (brandId < 0)
                return BadRequest("there are no brand with id lower than 0");
            if (prodNameSearch == "null")
                prodNameSearch = null;
            if(productId < 0)
                return BadRequest("product id can't be lower than 0");

            return Ok(_infoRequestService.GetPage(page,pageSize,brandId,prodNameSearch,isAsc,productId));

        }
        /// <summary>
        /// api for brand detail 
        /// </summary>
        /// <param name="id">id of the brand</param>
        /// <returns></returns>
        [Route("Detail/{id}")]
        async public Task<IActionResult> GetInfoRequestDetail(int id)
        {
            if (id <= 0)
                return BadRequest("id can't be lower or equal than 0");
            return Ok(await _infoRequestService.GetInfoRequestDetail(id));
        }


    }
}
