using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using DataLayer.Repository;
using Domain;
using Domain.ModelsForApi;
using Microsoft.EntityFrameworkCore;
using ServicaLayer.BrandService.Model;
using ServicaLayer.BrandService.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicaLayer.BrandService
{
    public class BrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IRepository<Category> _categoryRepository;
        public BrandService(IBrandRepository brandRepository,IRepository<Category> categoryRepository)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }
        /// <summary>
        /// get all data needed for a brand page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public BrandPageDTO GetBrandPage(int page, int pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            var brandPageModel = new BrandPageDTO
            {
                Brands = _brandRepository.GetAll().OrderByDescending(x => x.Id).Page(page,pageSize).MapBrandForBrandPage(),
                Page = page,
                PageSize = pageSize,
                TotalBrand = _brandRepository.GetAll().Count(),
            };
            
            brandPageModel.TotalPages = CalculateTotalPages(brandPageModel.TotalBrand, pageSize);

            return brandPageModel;
        }
        /// <summary>
        /// get detail data for a brand
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        async public Task<BrandDetailDTO> GetBrandDetail(int id)
        {
            
            var query = _brandRepository.GetById(id).Select(b => new BrandDetailDTO
            {
                Id = b.Id,
                Name = b.BrandName,
                CountRequestFromBrandProducts = b.Products.SelectMany(x => x.InfoRequests).Count(),
                TotProducts = b.Products.Count(),
                Products = b.Products.Select(product => new ProductBrandDetailDTO
                {
                    Id = product.Id,
                    CountInfoRequest = product.InfoRequests.Count(),
                    Name = product.Name,

                }),
                AssociatedCategory = _categoryRepository.GetAll().Where(x => b.Products.SelectMany(c => c.ProductCategories).Select(b => b.CategoryId).Contains(x.Id)).Select(ca => new CategoryBrandDetailDTO
                {
                    Id = ca.Id,
                    Name = ca.Name,
                    CountProdAssociatied = b.Products.SelectMany(x => x.ProductCategories).Where(d => d.CategoryId == ca.Id).Count(),
                }),

            });

            return await query.FirstOrDefaultAsync();

        }
        /// <summary>
        /// insert a brand with his account and all his products with categories
        /// </summary>
        /// <param name="account"></param>
        /// <param name="brand"></param>
        /// <param name="prodsWithCat">products and list of categories id associated</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">account, brand , prods array null</exception>
        public async Task<int> InsertBrand(Account account,Brand brand, ProdWithCat[] prodsWithCat)
        {
            if(account == null)
                throw new ArgumentNullException(nameof(account));
            if(brand == null)
                throw new ArgumentNullException(nameof(brand));
            if(prodsWithCat == null)
                throw new ArgumentNullException(nameof(prodsWithCat));
            if(ValidateBrandInsert(new BrandInsertApiModel { Account = account, Brand = brand,prodsWithCats=prodsWithCat }) != null)
                throw new InvalidOperationException("Data are not valid");

            return await _brandRepository.InsertWithProducts(account,brand, prodsWithCat);
        }

        /// <summary>
        /// deletes a brand and all related data
        /// </summary>
        /// <param name="id">id brand</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id not valid</exception>
        public async Task<bool> DeleteAll(int id)
        {
            if(id<=0)
                throw new ArgumentOutOfRangeException(nameof(id));
            
            return await _brandRepository.DeleteAll(id);
        }
        /// <summary>
        /// edit data of a brand
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input bran dis null</exception>
        /// <exception cref="NullReferenceException">brand id is not valid</exception>
        public async Task<bool> EditBrand(Brand brand)
        {
            if(brand == null)
                throw new ArgumentNullException(nameof(brand));
            if (ValidateBrandUpdate(brand) != null)
                throw new InvalidOperationException("brand data was not valid");
            Brand BrandFromRepo=_brandRepository.GetById(brand.Id).FirstOrDefault();
            if (BrandFromRepo == null)
                throw new NullReferenceException("id not valid");

            BrandFromRepo.BrandName = brand.BrandName;
            BrandFromRepo.Description = brand.Description;
            if (await _brandRepository.Update(BrandFromRepo) > 0)
                return true;

            return false;
        }
        /// <summary>
        /// get brand from repo and return it
        /// </summary>
        /// <param name="id">brand id</param>
        /// <returns>null or the brand selected</returns>
        /// <exception cref="ArgumentOutOfRangeException">brand id is not valid</exception>
        public async Task<Brand> GetBrand(int id)
        {
            if(id <=0)
                throw new ArgumentOutOfRangeException(nameof(id));
            return await _brandRepository.GetById(id).FirstOrDefaultAsync();
        }
        public async Task<bool> ValidateExistsEmail(string email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            if (email.Length == 0 || email.Length > 255)
                throw new ArgumentException(nameof(email));
            if (!IsValidEmail(email))
                throw new ArgumentOutOfRangeException("email pattern not valid", nameof(email));
            return await _brandRepository.ValidateEmailExistence(email);
        }




        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
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

            if (brandInsertApiModel.Account.Email.Length == 0 || brandInsertApiModel.Account.Email.Length > 255)
                result += "Email can't be empity and can't have more than 255 charaters \n";
            if (brandInsertApiModel.Account.Password.Length == 0 || brandInsertApiModel.Account.Password.Length > 18)
                result += "Password can't be empity or can't have more than 18 characters \n";
            if (brandInsertApiModel.Brand.BrandName.Length == 0 || brandInsertApiModel.Brand.BrandName.Length > 255)
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
                if (prod.Product.Price < 0 || prod.Product.Price > (decimal)1e16)
                {
                    result += "price can't be lower than 0 or higher than 1e16 \n";
                    break;
                }
            }

            return result;
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
            if (brand.BrandName.Length == 0 && brand.BrandName.Length > 255)
                result = "Not valid Brand Name it was empity string or a string with more than 255 characters";

            return result;
        }
        /// <summary>
        /// tells if email pattern is valid
        /// https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address?page=1&tab=votes#tab-top
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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


















        //TOTO remove test not working ---------------------------------------------------------------------------------------------
        public BrandPageDTO GetBrandPage2(int page, int pageSize)//in progress
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            var query = _brandRepository.GetAll().Select(b => new BrandPageDTO
            {
                Brands = _brandRepository.GetAll().OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).MapBrandForBrandPage(),
                Page = page,
                PageSize = pageSize,
                TotalBrand = _brandRepository.GetAll().Count(),
            });
            BrandPageDTO brandPageModel = query.FirstOrDefault();

            brandPageModel.TotalPages = CalculateTotalPages(brandPageModel.TotalBrand, pageSize);

            return brandPageModel;
        }

        public BrandDetailDTO GetBrandDetail2(int id)//in progress
        {
            
            var query = _brandRepository.GetById(id).Select(b => new BrandDetailDTO
            {
                Id = b.Id,
                Name = b.BrandName,
                CountRequestFromBrandProducts = b.Products.SelectMany(x => x.InfoRequests).Count(),
                TotProducts = b.Products.Count(),
                Products = b.Products.AsQueryable<Product>().MapProductsForBrandDetail(),
                AssociatedCategory = _categoryRepository.GetAll().Where(x => b.Products.SelectMany(c => c.ProductCategories).Select(b => b.CategoryId).Contains(x.Id)).Select(ca => new CategoryBrandDetailDTO
                {
                    Id = ca.Id,
                    Name = ca.Name,
                    CountProdAssociatied = b.Products.SelectMany(x => x.ProductCategories).Where(d => d.CategoryId == ca.Id).Count(),
                }),

            });

            return query.FirstOrDefault();

        }

        
    }
}
