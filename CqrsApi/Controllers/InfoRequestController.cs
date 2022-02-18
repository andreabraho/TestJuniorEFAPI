using CqrsServices.Queries.InfoRequestQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CqrsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoRequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InfoRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// api for brand detail 
        /// </summary>
        /// <param name="id">id of the brand</param>
        /// <returns></returns>
        [Route("Detail/{id}")]
        async public Task<IActionResult> GetInfoRequestDetail(int id)
        {
            var response = await _mediator.Send(new GetInfoRequestDetailById.Query(id));
               
            return Ok(response);
        }
        /// <summary>
        /// gets all data needed for info request page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="brandId"></param>
        /// <param name="prodNameSearch">search done by user</param>
        /// <param name="isAsc"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [Route("Page/{page:int=1}/{pageSize:int=10}")]
        public async Task<IActionResult> GetPage(int page, int pageSize, int brandId, string prodNameSearch, bool isAsc, int productId)
        {
            var response= await _mediator.Send(new GetInfoRequestPage.Query(page, pageSize, brandId, prodNameSearch, isAsc, productId));
                
            return Ok(response);

        }
    }
}
