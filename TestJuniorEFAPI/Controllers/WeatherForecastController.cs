using Domain;
using Domain.APIModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestJuniorEFAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private MyContext _ctx;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MyContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("Get")]
        public IActionResult Get()
        {


            //var productList = _ctx.Products
            //    .Include(x => x.Product_Categories)
            //        .ThenInclude(x => x.Category)
            //    .Select(product => new ProductBla
            //    {
            //        Id = product.Id,
            //        Name = product.Name,
            //        ProductCategories = product.Product_Categories.Select(productCategory => new ProductCategoryBla
            //        {
            //            CategoryId = productCategory.CategoryId,
            //            ProductId = productCategory.ProductId,
            //            Category=productCategory.Category,
            //        })
            //    })
            //    .ToList();



            /*
             Select C.Id,C.Name,Count(*) as ProdAsscociatedToCat
            From Product as P Join Product_Category as PC ON P.Id=PC.ProductId
		            JOIN Category As C ON PC.CategoryId=C.Id
            WHERE P.BrandId=1
            Group BY C.Id,C.Name 
            */
            
            var query = from p in _ctx.Set<Product>()
                        join pc in _ctx.Set<Product_Category>() on p.Id equals pc.ProductId
                        join c in _ctx.Set<Category>() on pc.CategoryId equals c.Id
                        where p.BrandId==1
                        group p by c.Id
            into g
                        select new { g.Key, Count = g.Count() };
                        
                        

            var t =query.ToList();
            return Ok(query);
        }
    }

    public class ProductBla
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<ProductCategoryBla> ProductCategories { get; set; } = new List<ProductCategoryBla>();
    }

    public class ProductCategoryBla
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public Category Category { get; set; }
    }
}
