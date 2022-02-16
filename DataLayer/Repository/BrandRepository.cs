using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using Domain.ModelsForApi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(MyContext context) : base(context) { }
        /// <summary>
        /// insert a brand(his account also) and insert andy number of products with any number of categories
        /// </summary>
        /// <param name="account"></param>
        /// <param name="brand"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">account null</exception>
        /// <exception cref="ArgumentNullException">brand null</exception>
        /// <exception cref="ArgumentNullException">products null</exception>
        public async Task<int> InsertWithProducts(Account account,Brand brand, ProdWithCat[] products)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (brand == null)
                throw new ArgumentNullException(nameof(brand));
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            brand.Account = account;

            IEnumerable<Product> productsList = products.MapToProducts();

            brand.Products = productsList;

            try
            {
                _ctx.Brands.Add(brand);
                await _ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            return brand.Id;
        }
        /// <summary>
        /// soft delete a brand and all main branch data related to him
        /// </summary>
        /// <param name="id">brand id</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id not valid</exception>
        public async Task<bool> DeleteBrandAndRelatedData(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "id can't be lower or equal than 0");
            
            var result = true;
            try
            {
                //await _ctx.Database.ExecuteSqlRawAsync(@"update InfoRequest
                //                                        Set InfoRequest.IsDeleted=1
                //                                        From InfoRequest as ir join Product as p On ir.ProductId=p.Id
                //                                        where p.BrandId=" + id
                //                                            );
                //await _ctx.Database.ExecuteSqlRawAsync(@"update Product
                //                                        Set IsDeleted=1
                //                                        where BrandId=
                //                                        " + id);
                await _ctx.InfoRequests.Where(x => x.Product.BrandId == id).UpdateFromQueryAsync(x => new InfoRequest()
                {
                    IsDeleted = true,
                });
                await _ctx.Products.Where(x => x.BrandId == id).UpdateFromQueryAsync(x => new Product()
                {
                    IsDeleted = true,
                });

                await _ctx.SaveChangesAsync();
                await DeleteAsync(id);
            }
            catch (Exception e)
            {
                result=false;
            }
            return result;
        }
        /// <summary>
        /// method that check if email already exists on database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> ValidateEmailExistence(string email)
        {
            var result=await _ctx.Accounts.FirstOrDefaultAsync(x=>x.Email == email);
            if (result == null)
                return true;
            return false;
        }

    }
    
}
