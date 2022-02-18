using CqrsServices.Validation;
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
        public class Validator : IValidationHandler<Query>
        {
            public async Task<ValidationResult> Validate(Query request)
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                    return ValidationResult.Fail("Email can't be Null or Empity");
                if (request.Email.Length > 255)
                    return ValidationResult.Fail("Email can't have more than 255 Characters");
                if (!IsValidEmail(request.Email))
                    return ValidationResult.Fail("Email pattern not valid");

                return ValidationResult.Success;
            }
            private bool IsValidEmail(string email)
            {
                var trimmedEmail = email.Trim();

                if (trimmedEmail.EndsWith("."))
                {
                    return false;
                }
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == trimmedEmail;
                }
                catch
                {
                    return false;
                }
            }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IBrandRepository _brandRepository;
            public Handler(IBrandRepository brandRepository)
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
