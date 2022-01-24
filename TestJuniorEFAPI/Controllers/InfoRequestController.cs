using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestJuniorEFAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoRequestController : ControllerBase
    {
        ILogger<InfoRequest> _logger;
        private IInfoRequestRepository _infoRequestRepository;
        public InfoRequestController(ILogger<InfoRequest> logger, IInfoRequestRepository infoRequestRepository)
        {
            this._infoRequestRepository = infoRequestRepository;
            this._logger = logger;
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
            return Ok(_infoRequestRepository.InfoRequestDetailV2(id));
        }


    }
}
