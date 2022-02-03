﻿using DataLayer.Interfaces;
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
        [Route("Page/{page:int=1}/{pageSize:int=10}/{brandId:int=0}/{prodNameSearch=}/{isAsc:bool=false}")]
        public IActionResult GetPage(int page,int pageSize,int brandId,string prodNameSearch,bool isAsc)
        {
            if (page <= 0)
                return NotFound("page not found");
            if (pageSize <= 0 || pageSize > 1000)
                return BadRequest("page size can't be lower or equal than 0 or higher than 1000");
            if (brandId < 0)
                return BadRequest("there are no brand with id lower than 0");

            return Ok(_infoRequestService.GetPage(page,pageSize,brandId,prodNameSearch,isAsc));

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
