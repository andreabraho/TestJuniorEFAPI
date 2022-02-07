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
        public async Task<bool> InsertBrand(Account account,Brand brand, ProdWithCat[] prodsWithCat)
        {
            if(account == null)
                throw new ArgumentNullException(nameof(account));
            if(brand == null)
                throw new ArgumentNullException(nameof(brand));
            if(prodsWithCat == null)
                throw new ArgumentNullException(nameof(prodsWithCat));


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
        public async Task<bool> EditBrand(Brand brand)
        {
            Brand BrandFromRepo=_brandRepository.GetById(brand.Id).FirstOrDefault();
            if (BrandFromRepo == null)
                throw new NullReferenceException("id not valid");

            BrandFromRepo.BrandName = brand.BrandName;

            if (await _brandRepository.Update(BrandFromRepo) > 0)
                return true;

            return false;
        }







        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }





















        //test not working ---------------------------------------------------------------------------------------------
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
