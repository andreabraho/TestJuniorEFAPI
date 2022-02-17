using DataLayer.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Queries.BrandQueries
{
    public static class ValidateEmail
    {
        public class Query : IRequest<Response>
        {
            public string Email { get; set; }
            public Query(string email)
            {
                Email = email;
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
                return new Response { Result = await _brandRepository.ValidateEmailExistence(request.Email) };
            }
        }
        public class Response : CQRSResponse
        {
            public bool Result { get; set; }
        }
    }
}
