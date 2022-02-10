using DataLayer.Interfaces;
using Domain;
using Domain.ModelsForApi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if(account == null)
                throw new ArgumentNullException(nameof(account));
            if(brand == null)
                throw new ArgumentNullException(nameof(brand));
            if(products == null)
                throw new ArgumentNullException(nameof(products));

            var result = true;

            _ctx.Accounts.Add(account);
            if(await _ctx.SaveChangesAsync()<=0)//if account insert fails set resul false so next streps are skipped
                result= false;

            brand.AccountId = account.Id;

            _ctx.Brands.Add(brand);
            if(await _ctx.SaveChangesAsync()<=0 && result)//if brand insert fails and account was insterted remove account and set result false
            {
                result = false;
                _ctx.Accounts.Remove(account);
                await _ctx.SaveChangesAsync();
            }


            if (products.Length > 0 && result)
            {
                foreach (var product in products)
                {
                    product.Product.BrandId=brand.Id;

                    await _ctx.Products.AddAsync(product.Product);

                    if (await _ctx.SaveChangesAsync() > 0 && product.CategoriesIds.Length>0)//if product is inserted and there are categories
                    {
                        foreach (var catId in product.CategoriesIds)
                        {
                            await _ctx.Products_Categories.AddAsync(new ProductCategory { CategoryId = catId, ProductId = product.Product.Id });
                        }
                        await _ctx.SaveChangesAsync();
                    }
                }
            }

            return brand.Id;
        }

        /// <summary>
        /// soft delete a brand and all data related to him
        /// </summary>
        /// <param name="id">brand id</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id not valid</exception>
        public async Task<bool> DeleteAll(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "id can't be lower or equal than 0");
            
            var result = true;

            await _ctx.Database.ExecuteSqlRawAsync(@"update InfoRequestReply  
                                                        Set InfoRequestReply.IsDeleted=1 
                                                        From InfoRequestReply as irr 
                                                            join InfoRequest as ir On irr.InfoRequestId=ir.Id 
                                                            join Product as p On ir.ProductId=p.Id 
                                                        where p.BrandId="+id
                                                        );
            await _ctx.Database.ExecuteSqlRawAsync(@"update InfoRequest
                                                        Set InfoRequest.IsDeleted=1
                                                        From InfoRequest as ir join Product as p On ir.ProductId=p.Id
                                                        where p.BrandId="+id
                                                        );
            await _ctx.Database.ExecuteSqlRawAsync(@"update Product
                                                        Set IsDeleted=1
                                                        where BrandId=
                                                        "+id);
            
            await _ctx.SaveChangesAsync();

            await DeleteAsync(id);

            return result;
        }
        public async Task<bool> ExistsEmail(string email)
        {
            var result=await _ctx.Accounts.FirstOrDefaultAsync(x=>x.Email == email);
            if (result == null)
                return true;
            return false;
        }

    }
    
}
