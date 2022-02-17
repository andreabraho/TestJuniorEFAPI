using CqrsServices.Queries.BrandQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CqrsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
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
        async public Task<IActionResult> GetBrandDetail(int id)
        {
            var response = await _mediator.Send(new GetBrandDetailById.Query(id));
            if(response!=null)
                return Ok(response);
            else
                return NotFound();
        }
        /// <summary>
        /// get all info needed for a brand page
        /// </summary>
        /// <param name="page">optional,default 1,type int,rappresents page needed </param>
        /// <param name="pageSize">optional,default 10,type int,rappresents page dimensione</param>
        /// <returns></returns>
        [Route("Page/{page:int=1}/{pageSize:int=10}")]
        public async Task<IActionResult> GetBrandPage(int page, int pageSize)
        {
            var response =await _mediator.Send(new GetBrandPage.Query(page, pageSize));

            if (response != null)
                return Ok(response);
            else
                return NotFound();
        }





    }
}
