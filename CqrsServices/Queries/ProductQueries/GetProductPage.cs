using CqrsServices.Validation;
using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Queries
{
    public static class GetProductPage
    {
        public class Query : IRequest<Response>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int BrandId { get; set; }
            public OrderProduct OrderBy { get; set; }
            public bool IsAsc { get; set; }
            public Query(int page,int pageSize,int brandId,int orderBy,bool isAsc)
            {
                Page = page;
                PageSize = pageSize;
                BrandId = brandId;
                OrderBy = (OrderProduct)orderBy;
                IsAsc = isAsc;
            }
            
        }
        public class Validator : IValidationHandler<Query>
        {
            public async Task<ValidationResult> Validate(Query request)
            {
                string result = null;
                result = ValidatePage(request);

                if (result != null)
                    return ValidationResult.Fail(result);

                return ValidationResult.Success;
            }

            private static string ValidatePage(Query request)
            {
                string result = null;
                if (request.PageSize <= 0)
                    result += "Page size can't be equal or lower than 0";
                if (request.Page <= 0)
                    result += "Page can't be equal or lower than 0";
                if (request.BrandId < 0)
                    result += "Brand Id can't be equal or lower than 0";
                return result;
            }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IProductRepository _productRepository;
            private readonly IBrandRepository _brandRepository;
            private readonly IRepository<Category> _categoryRepository;
            public Handler(IProductRepository productRepository, IBrandRepository brandRepository, IRepository<Category> categoryRepository)
            {
                _productRepository = productRepository;
                _brandRepository = brandRepository;
                _categoryRepository = categoryRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new Response
                {
                    PageSize = request.PageSize,
                    Page = request.Page,
                    TotalProducts = _productRepository.GetAll().FilterProducts(request.BrandId).Count(),
                    Brands = _brandRepository.GetAll().Select(b => new BrandForPageDTO
                    {
                        Id = b.Id,
                        Name = b.BrandName
                    })

                };
                var query = _productRepository.GetAll();

                query = query.FilterProducts(request.BrandId);

                query = query.OrderForPage(request.OrderBy, request.IsAsc);

                query = query.Page(request.Page, request.PageSize);

                response.Products = query.MapProductsForPage();

                response.TotalPages = CalculateTotalPages(response.TotalProducts, request.PageSize);

                return response;
            }
        }














        public class Response:CQRSResponse
        {
            /// <summary>
            /// page needed
            /// </summary>
            public int Page { get; set; }
            /// <summary>
            /// number of products for each page
            /// </summary>
            public int PageSize { get; set; }
            /// <summary>
            /// total number of products
            /// </summary>
            public int TotalProducts { get; set; }
            public int TotalPages { get; set; }
            /// <summary>
            /// list of products for the page
            /// </summary>
            public IEnumerable<ProductForPageDTO> Products { get; set; }
            public IEnumerable<BrandForPageDTO> Brands { get; set; }
        }
        /// <summary>
        /// data needed for each product for the product paging api
        /// </summary>
        public class ProductForPageDTO
        {
            public int Id { get; set; }
            /// <summary>
            /// product name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// product image link
            /// </summary>
            public string Image { get; set; }
            /// <summary>
            /// product short description
            /// </summary>
            public string ShortDescription { get; set; }
            public int BrandId { get; set; }
            public string BrandName { get; set; }
            public decimal Price { get; set; }

            public ICollection<Category> Categories { get; set; }
        }
        public class BrandForPageDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }





        public static IQueryable<Product> FilterProducts(this IQueryable<Product> products, int brandId = 0)
        {
            if (brandId > 0)
            {
                products = products.Where(x => x.BrandId == brandId);
            }
            return products;
        }
        public static IQueryable<Product> OrderForPage(this IQueryable<Product> products, OrderProduct orderBy, bool isAsc)
        {
            switch (orderBy)
            {
                case OrderProduct.BrandName:
                    if (isAsc)
                        products = products.OrderBy(x => x.Brand.BrandName);
                    else
                        products = products.OrderByDescending(x => x.Brand.BrandName);
                    break;
                case OrderProduct.ProductName:
                    if (isAsc)
                        products = products.OrderBy(x => x.Name);
                    else
                        products = products.OrderByDescending(x => x.Name);
                    break;
                case OrderProduct.Price:
                    if (isAsc)
                        products = products.OrderBy(x => x.Price);
                    else
                        products = products.OrderByDescending(x => x.Price);
                    break;
                default:
                    products = products.OrderBy(x => x.Brand.BrandName).ThenBy(x => x.Name);
                    break;
            }
            return products;
        }
        public static IQueryable<ProductForPageDTO> MapProductsForPage(this IQueryable<Product> products)
        {
            return products.Select(product => new ProductForPageDTO
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.GetFakeImage(),
                ShortDescription = product.ShortDescription,
                BrandId = product.BrandId,
                BrandName = product.Brand.BrandName,
                Categories = product.ProductCategories.Select(c => c.Category).ToList(),
                Price = product.Price,
            });


        }
        internal static int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }
    }
}
