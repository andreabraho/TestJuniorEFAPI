using CqrsServices.Validation;
using DataLayer.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Queries.ProductQueries
{
    public static class GetDataForUpdate
    {
        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
            public Query(int id)
            {
                Id = id;
            }
        }

        public class Validator : IValidationHandler<Query>
        {
            public async Task<ValidationResult> Validate(Query request)
            {
                if (request.Id <= 0)
                    return ValidationResult.Fail("Id can't be lower or equal than 0");
                return ValidationResult.Success;
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
                var response = _productRepository.GetById(request.Id).Include(x => x.ProductCategories).Select(x => new Response
                {
                    AllBrands = _brandRepository.GetAll().Select(brand => new BrandForUpdatetDTO
                    {
                        Id = brand.Id,
                        Name = brand.BrandName
                    }),
                    AllCategories = _categoryRepository.GetAll().Select(cat => new CatForUpdateDTO
                    {
                        Name = cat.Name,
                        Id = cat.Id,
                    }),
                    CategoriesAssociated = x.ProductCategories.Select(cat => cat.CategoryId).ToArray(),
                    Product = _productRepository.GetById(request.Id).FirstOrDefault()

                }).FirstOrDefault();

                return response;
            }
        }
        public class Response:CQRSResponse
        {
            public Product Product { get; set; }
            public int[] CategoriesAssociated { get; set; }
            public IEnumerable<BrandForUpdatetDTO> AllBrands { get; set; }
            public IEnumerable<CatForUpdateDTO> AllCategories { get; set; }
        }
        public class BrandForUpdatetDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class CatForUpdateDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
