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
        public class Query : IRequest<bool>
        {
            public string Email { get; set; }
            public Query(string email)
            {
                Email = email;
            }

        }


        public class Handaler : IRequestHandler<Query, bool>
        {
            private readonly IBrandRepository _brandRepository;
            public Handaler(IBrandRepository brandRepository)
            {
                _brandRepository = brandRepository;
            }
            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _brandRepository.ValidateEmailExistence(request.Email);
            }
        }



    }
}
