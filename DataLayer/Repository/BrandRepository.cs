using DataLayer.Interfaces;
using Domain;
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
        public async Task<bool> InsertWithProducts(Account account,Brand brand, ProdWithCat[] products)
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
            if(await _ctx.SaveChangesAsync()<=0 && result)//if brand insert fails remove account and set result false
            {
                result = false;
                _ctx.Accounts.Remove(account);
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

            return result;
        }

    }
    public class ProdWithCat
    {
        public Product Product { get; set; }
        public int[] CategoriesIds  { get; set; }
    }
}
