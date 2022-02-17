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
        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
            public Query(int id)
            {
                Id = id;
            }
        }


        public class Handaler : IRequestHandler<Query, Response>
        {
            private readonly IBrandRepository _brandRepository;
            public Handaler(IBrandRepository brandRepository)
            {
                _brandRepository = brandRepository;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response { Brand = await _brandRepository.GetById(request.Id).FirstOrDefaultAsync() };
            }
        }
        public class Response:CQRSResponse
        {
            public Brand Brand { get; set; }
        }
    }
}
