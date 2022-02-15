using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using ServicaLayer.BrandService;
using System.Threading.Tasks;
using DataLayer.Repository;
using System.Collections.Generic;
using Domain.ModelsForApi;

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
        async public Task<IActionResult> GetBrandDetail(int id)
        {
            if (id <= 0)
                return BadRequest("id not valid");

            return Ok( await _brandService.GetBrandDetail(id));
        }
        /// <summary>
        /// inserts brand with and array of products each one with different categories
        /// </summary>
        /// <param name="testModel">model containing all needed data to insert the brand with products </param>
        /// <returns></returns>
        [HttpPost("Insert")]
        public async Task<IActionResult> InsertBrand(BrandInsertApiModel testModel)
        {
            if (testModel == null)
                return BadRequest("brand model was null give valid data");

            string res = ValidateBrandInsert(testModel);
            if (res != null)
                return BadRequest(res);

            int result = await _brandService.InsertBrand(testModel.Account, testModel.Brand, testModel.prodsWithCats);
            if (result>0)
                return Ok(result);
            return NoContent();
        }
        /// <summary>
        /// deletes the brand and all data related to him
        /// </summary>
        /// <param name="id">brand id</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBrandAndRelations(int id)
        {
            if (id <= 0)
                return BadRequest();

            var result = await _brandService.DeleteAll(id);
            if (result)
                return Ok(result);

            return NotFound(result);
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
            if (ValidateBrandUpdate(brand) != null)
                return BadRequest(ValidateBrandUpdate(brand));


            if (await _brandService.EditBrand(brand))
                return Ok(brand);
            return NoContent();
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

            var result=await _brandService.GetBrand(id);
            if(result!=null)
                return Ok(result);
            return NoContent();
        }
        /// <summary>
        /// check if the email is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true or false</returns>
        [HttpGet("ValidateMail/{email=}")]
        public async Task<IActionResult> ValidateMail(string email)
        {
            if(email==null)
                return Ok(true);
            return Ok(await _brandService.ExistsEmail(email));
        }





        /// <summary>
        /// validates the model in input for brand Update api
        /// </summary>
        /// <param name="brand"></param>
        /// <returns>null if the model is valid,
        /// string with error if not</returns>
        private string ValidateBrandUpdate(Brand brand)
        {
            string result = null;
            if (brand.BrandName.Length == 0 && brand.BrandName.Length>255)
                result = "Not valid Brand Name it was empity string or a string with more than 255 characters";
            
            
            return result;
        }
        /// <summary>
        /// validates the model in input for brand insert api
        /// </summary>
        /// <param name="brandInsertApiModel"></param>
        /// <returns>null if the model is valid,
        /// string with error if not</returns>
        private string ValidateBrandInsert(BrandInsertApiModel brandInsertApiModel)
        {
            string result = null;

            if (brandInsertApiModel.Account.Email.Length == 0 || brandInsertApiModel.Account.Email.Length>255)
                result += "Email can't be empity and can't have more than 255 charaters \n";
            if (brandInsertApiModel.Account.Password.Length == 0 || brandInsertApiModel.Account.Password.Length>18)
                result += "Password can't be empity or can't have more than 18 characters \n";
            if (brandInsertApiModel.Brand.BrandName.Length == 0 || brandInsertApiModel.Brand.BrandName.Length >255)
                result = "Brand name can't be empity or can't have more than 255 characters \n";
            if (!IsValidEmail(brandInsertApiModel.Account.Email))
                result += "email pattern is not valid";
            foreach (ProdWithCat prod in brandInsertApiModel.prodsWithCats)
                if (prod.CategoriesIds.Length == 0)
                {
                    result += "Select at least one category for each product \n";
                    break;
                }
            
            foreach (ProdWithCat prod in brandInsertApiModel.prodsWithCats)
            {
                if (prod.Product.Name.Length == 0)
                {
                    result += "Product names can't be empity \n";
                    break;
                }
            }
            foreach (ProdWithCat prod in brandInsertApiModel.prodsWithCats)
            {
                if (prod.Product.ShortDescription.Length == 0)
                {
                    result += "Products short description can't be empity \n";
                    break;
                }
            }
            foreach (ProdWithCat prod in brandInsertApiModel.prodsWithCats)
            {
                if (prod.Product.Price<0 || prod.Product.Price>(decimal)1e16)
                {
                    result += "price can't be lower than 0 or higher than 1e16 \n";
                    break;
                }
            }

            return result;
        }
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
