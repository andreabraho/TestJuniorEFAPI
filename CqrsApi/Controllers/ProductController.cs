using CqrsServices.Commands;
using CqrsServices.Commands.ProductCommands;
using CqrsServices.Queries;
using CqrsServices.Queries.ProductQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TestJuniorEFAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Route("page/{page:int}/{pagesize:int}")]
        public async Task<IActionResult> GetPage(int page, int pageSize, int brandId, int orderBy, bool isAsc)
        {
            var response = await _mediator.Send(new GetProductPage.Query(page, pageSize, brandId, orderBy, isAsc));
               
            return Ok(response);
        }
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var response=await _mediator.Send(new GetProductDetailById.Query(id));
                
            return Ok(response);
        }
        [HttpPost("Upsert")]
        [HttpPut("Upsert")]
        public async Task<IActionResult> Upsert(UpsertProduct.Command command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// deletes a product and all data in rlation with him
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {


            var result =await _mediator.Send(new DeleteProduct.Command(id));

            return Ok(result.Result);
        }


        /// <summary>
        /// method that gets all data neccessary to insert a product
        /// </summary>
        /// <returns></returns>
        [HttpGet("Insert")]
        public async Task<IActionResult> GetCatListBrandList()
        {
            var response = await _mediator.Send(new GetDataForInsert.Query());
                
            return Ok(response);
        }
        /// <summary>
        /// return the product data needed for update
        /// </summary>
        /// <param name="id">id of the product to be returned</param>
        /// <returns></returns>
        [HttpGet("Update/{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id)
        {
            var response = await _mediator.Send(new GetDataForUpdate.Query(id));
                
            return Ok(response);
        }
    }
}
