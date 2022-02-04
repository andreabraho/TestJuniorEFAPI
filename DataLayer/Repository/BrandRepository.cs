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

        public async Task<bool> InsertWithProducts(Brand brand, ProdWithCat[] products)
        {
            if(brand == null)
                throw new ArgumentNullException(nameof(brand));
            if(products == null)
                throw new ArgumentNullException(nameof(products));

            var result = true;


            _ctx.Brands.Add(brand);
            if(await _ctx.SaveChangesAsync()<=0)
                result = false;

            if(products.Length > 0)
            {
                foreach (var product in products)
                {
                    await _ctx.Products.AddAsync(product.Product);
                    if (await _ctx.SaveChangesAsync() > 0 && product.CategoriesIds.Length>0)
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
