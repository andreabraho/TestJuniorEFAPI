using DataLayer.Interfaces;
using Domain;
using Domain.APIModels;
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
        private readonly IRepository<Product> _productRepository;
        public BrandService(IRepository<Brand> brandRepository,IRepository<Category> categoryRepository, IRepository<Product> productRepository)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }
        public BrandPageModel GetBrandPage(int page, int pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            var query = _brandRepository.GetAll().Select(b => new BrandPageModel
            {
                Brands = _brandRepository.GetAll().OrderByDescending(x => x.Id).Skip(pageSize * (page - 1)).Take(pageSize).Select(brand => new BrandForPage
                {
                    IdProducts = brand.Products.Select(p => new ProductForBrandPage
                    {
                        Id = p.Id,
                    }),
                    Name = brand.BrandName,
                    Description = brand.Description,
                }),
                Page = page,
                PageSize = pageSize,
                TotalBrand = _brandRepository.GetAll().Count(),
            });
            BrandPageModel brandPageModel = query.FirstOrDefault();

            brandPageModel.TotalPages = CalculateTotalPages(brandPageModel.TotalBrand, pageSize);

            return brandPageModel;
        }
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

            brandPageModel.TotalPages = CalculateTotalPages(brandPageModel.TotalBrand,pageSize);

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

        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }
    }
}
