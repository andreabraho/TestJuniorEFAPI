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
        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
            public Command(int id)
            {
                Id=id;
            }


            public class Handaler : IRequestHandler<Command, bool>
            {
                private readonly IBrandRepository _brandRepository;
                public Handaler(IBrandRepository brandRepository)
                {
                    _brandRepository=brandRepository;
                }
                public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
                {
                    return await _brandRepository.DeleteBrandAndRelatedData(request.Id);
                }
            }


        }
    }
}
