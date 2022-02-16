using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(MyContext context) : base(context) { }
        /// <summary>
        /// upsert a product and his categories
        /// </summary>
        /// <param name="product"></param>
        /// <param name="cats">list of int rappresenting his categories</param>
        /// <returns>id of the product inserted</returns>
        /// <exception cref="ArgumentNullException">product null</exception>
        /// <exception cref="ArgumentNullException">cats null</exception>
        public async Task<int> Upsert(Product product,int[] cats)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (cats == null)
                throw new ArgumentNullException(nameof(cats));

            try
            {
                product.ProductCategories = cats.MapToProdCategory();

                if (product.Id == 0)
                    await _ctx.Products.AddAsync(product);
                else
                {
                    _ctx.Products_Categories.RemoveRange(_ctx.Products_Categories.Where(x=>x.ProductId == product.Id));
                    _ctx.Products.Update(product);
                }
                await _ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                product.Id = 0;
            }

            return product.Id;

        }

        /// <summary>
        /// soft delete a product and all main branch data related to him
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>true or false based on operation success</returns>
        /// <exception cref="ArgumentOutOfRangeException">id not valid</exception>
        public async Task<bool> DeleteProdAndRelatedData(int id)
        {
            if(id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id),"id can't be lower or equal than 0");
            
            var result = true;
            try
            {
                //await _ctx.Database.ExecuteSqlRawAsync(@"UPDATE InfoRequest
                //                                        SET InfoRequest.IsDeleted=1
                //                                        FROM InfoRequest as ir join Product as p On ir.ProductId=p.Id
                //                                        WHERE p.Id=" + id
                //                                            );

                await _ctx.InfoRequests.Where(x => x.ProductId == id).UpdateFromQueryAsync(x => new InfoRequest()
                {
                    IsDeleted = true,
                });
                
                await _ctx.SaveChangesAsync();
                await this.DeleteAsync(id);
            }
            catch(Exception e)
            {
                result= false;
            }
            return result;
        }
    }
}
