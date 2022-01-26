using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicaLayer.InfoRequestService;
using System.Linq;

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
        /// <summary>
        /// api for brand detail 
        /// </summary>
        /// <param name="id">id of the brand</param>
        /// <returns></returns>
        [Route("Detail/{id}")]
        public IActionResult GetInfoRequestDetail(int id)
        {
            if (id <= 0)
                return BadRequest("id can't be lower or equal than 0");
            return Ok(_infoRequestService.GetInfoRequestDetail(id));
        }


    }
}
