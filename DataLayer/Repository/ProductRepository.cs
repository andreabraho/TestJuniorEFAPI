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



            if(cats.Length > 0 && result)//if there are categories and the product was inserted
            {
                foreach (var cat in cats)
                {
                   await _ctx.Products_Categories.AddAsync(new ProductCategory { CategoryId = cat, ProductId = product.Id });
                }
                await _ctx.SaveChangesAsync();
                
            }
           
            return result;
        }

        /// <summary>
        /// soft delete a product and all data related to him
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id not valid</exception>
        public async Task<bool> DeleteAll(int id)
        {
            if(id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id),"id can't be lower or equal than 0");

            

            //foreach (var pc in _ctx.Products_Categories.Where(x => x.ProductId == id))//deletes product categories
            //    pc.IsDeleted = true;
            //foreach (var irr in _ctx.InfoRequestReplys.Where(x => x.InfoRequest.ProductId == id))//delete infoRequestReply
            //    irr.IsDeleted = true;
            //foreach (var ir in _ctx.InfoRequests.Where(x=>x.ProductId==id))//delete infoRequest
            //    ir.IsDeleted = true;

            await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequestReply  
                                                        SET InfoRequestReply.IsDeleted=1 
                                                        FROM InfoRequestReply as irr 
                                                            join InfoRequest as ir On irr.InfoRequestId=ir.Id 
                                                            join Product as p On ir.ProductId=p.Id 
                                                        WHERE p.Id=" + id
                                                        );
            await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequest
                                                        SET InfoRequest.IsDeleted=1
                                                        FROM InfoRequest as ir join Product as p On ir.ProductId=p.Id
                                                        WHERE p.Id=" + id
                                                        );

            await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE Product_Category
                                                        SET IsDeleted=1
                                                        WHERE ProductId=" + id);

            await _ctx.SaveChangesAsync();

            await this.DeleteAsync(id);


            var result = true;

            return result;
        }
    }
}
