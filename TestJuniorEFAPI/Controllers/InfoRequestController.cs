using Microsoft.AspNetCore.Mvc;

namespace TestJuniorEFAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoRequestController : ControllerBase
    {
        [Route("Detail/{id}")]
        public IActionResult GetInfoRequestDetail(int id)
        {

            

            return Ok();
        }


    }
}
