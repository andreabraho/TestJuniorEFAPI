using CqrsServices.Validation;
using DataLayer.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Commands.BrandCommands
{
    public static class BrandDelete
    {
        public class Command : IRequest<Response>
        {
            public int Id { get; set; }
            public Command(int id)
            {
                Id=id;
            }
        }
        public class Validator : IValidationHandler<Command>
        {
            public async Task<ValidationResult> Validate(Command request)
            {
                if (request.Id<= 0)
                    return ValidationResult.Fail("Id can't be lower or equal than 0");
                return ValidationResult.Success;
            }
        }

        public class Handaler : IRequestHandler<Command, Response>
        {
            private readonly IBrandRepository _brandRepository;
            public Handaler(IBrandRepository brandRepository)
            {
                _brandRepository = brandRepository;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                return new Response { Result = await _brandRepository.DeleteBrandAndRelatedData(request.Id) };
            }
        }
        public class Response : CQRSResponse
        {
            public bool Result { get; set; }
        }
    }
}
