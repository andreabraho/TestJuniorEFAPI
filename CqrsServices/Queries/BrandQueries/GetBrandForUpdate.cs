using DataLayer.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Queries.BrandQueries
{
    public static class GetBrandForUpdate
    {
        public class Query : IRequest<Brand>
        {
            public int Id { get; set; }
            public Query(int id)
            {
                Id = id;
            }
        }


        public class Handaler : IRequestHandler<Query, Brand>
        {
            private readonly IBrandRepository _brandRepository;
            public Handaler(IBrandRepository brandRepository)
            {
                _brandRepository = brandRepository;
            }
            public async Task<Brand> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _brandRepository.GetById(request.Id).FirstOrDefaultAsync();
            }
        }
    }
}
