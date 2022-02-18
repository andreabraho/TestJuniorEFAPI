using DataLayer.Interfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Queries.ProductQueries
{
    public static class GetDataForInsert
    {
        public class Query : IRequest<Response>
        {

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
                var response = new Response();
                response.Brands = _brandRepository.GetAll().Select(brand => new BrandForInsertDTO
                {
                    Id = brand.Id,
                    Name = brand.BrandName

                });
                response.Categories = _categoryRepository.GetAll().Select(cat => new CatForInsertDTO
                {
                    Id = cat.Id,
                    Name = cat.Name,
                });
                return response;
            }
        }

        public class Response:CQRSResponse
        {
            public IEnumerable<BrandForInsertDTO> Brands { get; set; }
            public IEnumerable<CatForInsertDTO> Categories { get; set; }

        }
        public class BrandForInsertDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class CatForInsertDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
