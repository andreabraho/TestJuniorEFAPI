using CqrsServices.Commands;
using CqrsServices.Commands.ProductCommands;
using CqrsServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TestJuniorEFAPI.Controllers
{
    [Route("Cqrs")]
    [ApiController]
    public class CqrsBaseController : Controller
    {
        private readonly IMediator _mediator;
        public CqrsBaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("Product/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var response=await _mediator.Send(new GetProductDetailById.Query(id));
            if(response!=null)
                return Ok(response);
            else
                return NotFound();
        }
        [HttpPost("Product/Upsert")]
        public async Task<IActionResult> Upsert(UpsertProduct.Command command)
        {
            var result = await _mediator.Send(command);
            if(result.Id!=0)
                return Ok(result.Id);
            else
                return BadRequest();
        }
    }
}
