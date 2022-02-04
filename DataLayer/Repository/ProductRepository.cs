using DataLayer.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(MyContext context) : base(context) { }
        /// <summary>
        /// insert a product and his categories
        /// </summary>
        /// <param name="product"></param>
        /// <param name="cats">list of int rappresenting his categories</param>
        /// <returns>bool for the result</returns>
        /// <exception cref="ArgumentNullException">product null</exception>
        /// <exception cref="ArgumentNullException">cats null</exception>
        public async Task<bool> InsertWithCat(Product product,int[] cats)
        {
            bool result = true;

            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (cats == null)
                throw new ArgumentNullException(nameof(cats));

            await _ctx.Products.AddAsync(product);
            
            if(await _ctx.SaveChangesAsync()<=0)//if not inserted
                result = false;



            if(cats.Length > 0 && result)//if there are cat and the product was inserted
            {
                foreach (var cat in cats)
                {
                    _ctx.Products_Categories.Add(new ProductCategory { CategoryId = cat, ProductId = product.Id });
                }
                await _ctx.SaveChangesAsync();
                if (await _ctx.SaveChangesAsync() <= 0)
                    result = false;
            }
           
            return result;
        }
    }
}
