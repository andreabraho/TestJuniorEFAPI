using CqrsServices.Commands.BrandCommands;
using CqrsServices.Queries.BrandQueries;
using Domain;
using Domain.ModelsForApi;
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

        /// <summary>
        /// inserts brand with and array of products each one with different categories
        /// </summary>
        /// <param name="testModel">model containing all needed data to insert the brand with products </param>
        /// <returns></returns>
        [HttpPost("Insert")]
        public async Task<IActionResult> InsertBrand(BrandInsertApiModel testModel)
        {
            var response = await _mediator.Send(new InsertBrand.Command(testModel.Account, testModel.Brand, testModel.prodsWithCats));
            if(response>0)
                return Ok(response);
            else
                return NotFound();
        }

        /// <summary>
        /// deletes the brand and all main branch data related to him
        /// </summary>
        /// <param name="id">brand id</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBrandAndRelations(int id)
        {
            var response=await _mediator.Send(new BrandDelete.Command(id));
            if(response)
                return Ok(response);
            else 
                return BadRequest(response);
        }
        /// <summary>
        /// updates a brand
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateBrand(Brand brand)
        {
            if (brand == null)
                BadRequest("brand was null");

            var response=await _mediator.Send(new BrandUpdate.Command(brand));
            if (response)
                return Ok(brand.Id);
            else
                return BadRequest(response);
        }
        /// <summary>
        /// get the brand data for brand update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Update/{id}")]
        public async Task<IActionResult> GetBrand(int id)
        {
            if (id <= 0)
                return BadRequest("id can't be lower or equal than 0");

            var response=await _mediator.Send(new GetBrandForUpdate.Query(id));
            if(response!=null)
                return Ok(response);
            else
                return NotFound();
            
        }
        /// <summary>
        /// check if the email is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true or false</returns>
        [HttpGet("ValidateMail/{email=}")]
        public async Task<IActionResult> ValidateMail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(false);
            if (email.Length == 0 || email.Length > 255)
                return BadRequest(false);
            if (!IsValidEmail(email))
                return BadRequest(false);

            var response=await _mediator.Send(new ValidateEmail.Query(email));
            if (response)
                return Ok(response);
            else
                return BadRequest();
            
        }
        /// <summary>
        /// tells if email pattern is valid
        /// https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address?page=1&tab=votes#tab-top
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
