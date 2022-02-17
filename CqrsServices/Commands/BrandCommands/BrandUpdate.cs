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

namespace CqrsServices.Commands.BrandCommands
{
    public static class BrandUpdate
    {
        public class Command : IRequest<bool>
        {
            public Brand Brand { get; set; }
            public Command(Brand brand)
            {
                Brand=brand;
            }
            public class Handaler : IRequestHandler<Command, bool>
            {
                private readonly IBrandRepository _brandRepository;
                public Handaler(IBrandRepository brandRepository)
                {
                    _brandRepository= brandRepository;
                }

                public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
                {
                    Brand BrandFromRepo =await _brandRepository.GetById(request.Brand.Id).FirstOrDefaultAsync();
                    if (BrandFromRepo == null)
                        throw new NullReferenceException("id not valid");

                    BrandFromRepo.BrandName = request.Brand.BrandName;
                    BrandFromRepo.Description = request.Brand.Description;
                    if (await _brandRepository.Update(BrandFromRepo) > 0)
                        return true;
                    else
                        return false;
                }
            }
        }
    }
}
