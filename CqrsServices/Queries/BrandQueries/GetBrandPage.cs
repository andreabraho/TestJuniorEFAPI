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

namespace CqrsServices.Queries.BrandQueries
{
    public static class GetBrandPage
    {
        public class Query : IRequest<Response>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public Query(int page,int pageSize)
            {
                Page = page;
                PageSize = pageSize;
            }
        }
        public class Validator : IValidationHandler<Query>
        {
            public async Task<ValidationResult> Validate(Query request)
            {
                string result = null;
                result = ValidatePage(request);
                if (result != null)
                    return ValidationResult.Fail("result");


                return ValidationResult.Success;
            }

            private static string ValidatePage(Query request)
            {
                string result=null;
                if (request.Page <= 0)
                    result += "Page can't be lower or equal than 0 \n";
                if (request.PageSize <= 0)
                    result += "Page Size can't be lower or equal than 0 \n";
                return result;
            }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IRepository<Category> _categoryRepository;
            public Handler(IBrandRepository brandRepository, IRepository<Category> categoryRepository)
            {
                _brandRepository = brandRepository;
                _categoryRepository = categoryRepository;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {

                var Response = new Response
                {
                    Brands = _brandRepository.GetAll().OrderByDescending(x => x.Id).Page(request.Page, request.PageSize).MapBrandForBrandPage(),
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalBrand = _brandRepository.GetAll().Count(),
                };

                Response.TotalPages = CalculateTotalPages(Response.TotalBrand, request.PageSize);

                return Response;

            }
        }

        public class Response:CQRSResponse
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int TotalBrand { get; set; }
            public IEnumerable<BrandForPageDTO> Brands { get; set; }
            public int TotalPages { get; set; }

        }
        /// <summary>
        /// data of each brand needed in brands page
        /// </summary>
        public class BrandForPageDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

        }
        public static IQueryable<BrandForPageDTO> MapBrandForBrandPage(this IQueryable<Brand> brands)
        {
            return brands.Select(brand => new BrandForPageDTO
            {
                Id = brand.Id,
                Name = brand.BrandName,
                Description = brand.Description,
                //IdProducts = brand.Products.AsQueryable().MapProductForPBrandPage(),
            });
        }
        private static int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }

    }
}
