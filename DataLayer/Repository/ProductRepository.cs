using DataLayer.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        /// <returns>id of the product inserted</returns>
        /// <exception cref="ArgumentNullException">product null</exception>
        /// <exception cref="ArgumentNullException">cats null</exception>
        public async Task<int> InsertWithCat(Product product,int[] cats)
        {
            bool result = true;

            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (cats == null)
                throw new ArgumentNullException(nameof(cats));


            IDbContextTransaction transaction = _ctx.Database.BeginTransaction();
            try
            {
                await _ctx.Products.AddAsync(product);

                if (await _ctx.SaveChangesAsync() <= 0)//if not inserted
                    result = false;



                if (cats.Length > 0 && result)//if there are categories and the product was inserted
                {
                    foreach (var cat in cats)
                    {
                        await _ctx.Products_Categories.AddAsync(new ProductCategory { CategoryId = cat, ProductId = product.Id });
                    }
                    await _ctx.SaveChangesAsync();

                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                result = false;
                transaction.Rollback();
            }

           
            return product.Id;
        }

        /// <summary>
        /// soft delete a product and all data related to him
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>true or false based on operation success</returns>
        /// <exception cref="ArgumentOutOfRangeException">id not valid</exception>
        public async Task<bool> DeleteAll(int id)
        {
            if(id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id),"id can't be lower or equal than 0");
            var result = true;
            try
            {
                //await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequestReply  
                //                                        SET InfoRequestReply.IsDeleted=1 
                //                                        FROM InfoRequestReply as irr 
                //                                            join InfoRequest as ir On irr.InfoRequestId=ir.Id 
                //                                            join Product as p On ir.ProductId=p.Id 
                //                                        WHERE p.Id=" + id
                //                                        );

                //await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequest
                //                                        SET InfoRequest.IsDeleted=1
                //                                        FROM InfoRequest as ir join Product as p On ir.ProductId=p.Id
                //                                        WHERE p.Id=" + id
                //                                            );

                //await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE Product_Category
                //                                        SET IsDeleted=1
                //                                        WHERE ProductId=" + id);

                await _ctx.InfoRequestReplys.Where(x => x.InfoRequest.ProductId == id).UpdateFromQueryAsync(x => new InfoRequestReply()
                {
                    IsDeleted = true,
                });
                
                await _ctx.InfoRequests.Where(x => x.ProductId == id).UpdateFromQueryAsync(x => new InfoRequest()
                {
                    IsDeleted = true,
                });
                
                await _ctx.Products_Categories.Where(x => x.ProductId == id).UpdateFromQueryAsync(x => new ProductCategory()
                {
                    IsDeleted = true,
                });
                await _ctx.SaveChangesAsync();

                await this.DeleteAsync(id);

                result = true;
            }
            catch(Exception e)
            {
                result= false;
            }
            return result;
        }
    }
}
