﻿using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using ServicaLayer.BrandService;

namespace TestJuniorEFAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly ILogger<Brand> _logger;
        private readonly BrandService _brandService;
        
        public BrandController(ILogger<Brand> logger, BrandService brandService)
        {
            _brandService=brandService;
            _logger = logger;
        }
        /// <summary>
        /// get all info needed for a brand page
        /// </summary>
        /// <param name="page">optional,default 1,type int,rappresents page needed </param>
        /// <param name="pageSize">optional,default 10,type int,rappresents page dimensione</param>
        /// <returns></returns>
        [Route("Page/{page:int=1}/{pageSize:int=10}")]
        public IActionResult GetBrandPage(int page, int pageSize)
        {
            if (page <= 0)
                return NotFound("page not found");
            if (pageSize <= 0|| pageSize>1000)
                return BadRequest("page size can't be lower or equal than 0 or higher than 1000");
            
            return Ok(_brandService.GetBrandPage(page,pageSize));
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

            return Ok(_brandService.GetBrandDetail(id));
        }







    }
}
