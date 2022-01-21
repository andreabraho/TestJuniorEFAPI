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
        [Route("Detail/{id}")]
        public IActionResult GetInfoRequestDetail(int id)
        {

            return Ok(_infoRequestRepository.InfoRequestDetailV2(id));
        }


    }
}
