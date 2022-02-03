using DataLayer.Interfaces;
using Domain;
using ServicaLayer.BrandService.Model;
using ServicaLayer.BrandService.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.BrandService
{
    public class BrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Category> _categoryRepository;
        public BrandService(IRepository<Brand> brandRepository,IRepository<Category> categoryRepository)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }
        public BrandPageModel GetBrandPage(int page, int pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            var brandPageModel = new BrandPageModel
            {
                Brands = _brandRepository.GetAll().OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).MapBrandForBrandPage(),
                Page = page,
                PageSize = pageSize,
                TotalBrand = _brandRepository.GetAll().Count(),
            };
            
            brandPageModel.TotalPages = CalculateTotalPages(brandPageModel.TotalBrand, pageSize);

            return brandPageModel;
        }
        
        public BrandDetail GetBrandDetail(int id)
        {
            
            var query = _brandRepository.GetById(id).Select(b => new BrandDetail
            {
                Id = b.Id,
                Name = b.BrandName,
                CountRequestFromBrandProducts = b.Products.SelectMany(x => x.InfoRequests).Count(),
                TotProducts = b.Products.Count(),
                Products = b.Products.Select(product => new ProductBrandDetail
                {
                    Id = product.Id,
                    CountInfoRequest = product.InfoRequests.Count(),
                    Name = product.Name,

                }),
                AssociatedCategory = _categoryRepository.GetAll().Where(x => b.Products.SelectMany(c => c.ProductCategories).Select(b => b.CategoryId).Contains(x.Id)).Select(ca => new CategoryBrandDetail
                {
                    Id = ca.Id,
                    Name = ca.Name,
                    CountProdAssociatied = b.Products.SelectMany(x => x.ProductCategories).Where(d => d.CategoryId == ca.Id).Count(),
                }),

            });

            return query.FirstOrDefault();

        }


        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }





















        //test not working ---------------------------------------------------------------------------------------------
        public BrandPageModel GetBrandPage2(int page, int pageSize)//in progress
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            var query = _brandRepository.GetAll().Select(b => new BrandPageModel
            {
                Brands = _brandRepository.GetAll().OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).MapBrandForBrandPage(),
                Page = page,
                PageSize = pageSize,
                TotalBrand = _brandRepository.GetAll().Count(),
            });
            BrandPageModel brandPageModel = query.FirstOrDefault();

            brandPageModel.TotalPages = CalculateTotalPages(brandPageModel.TotalBrand, pageSize);

            return brandPageModel;
        }

        public BrandDetail GetBrandDetail2(int id)//in progress
        {
            
            var query = _brandRepository.GetById(id).Select(b => new BrandDetail
            {
                Id = b.Id,
                Name = b.BrandName,
                CountRequestFromBrandProducts = b.Products.SelectMany(x => x.InfoRequests).Count(),
                TotProducts = b.Products.Count(),
                Products = b.Products.AsQueryable<Product>().MapProductsForBrandDetail(),
                AssociatedCategory = _categoryRepository.GetAll().Where(x => b.Products.SelectMany(c => c.ProductCategories).Select(b => b.CategoryId).Contains(x.Id)).Select(ca => new CategoryBrandDetail
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
