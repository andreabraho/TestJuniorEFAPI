using DataLayer.Interfaces;
using Domain;
using Domain.ModelsForApi;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Commands.BrandCommands
{
    public static class InsertBrand
    {
        public class Command : IRequest<int>
        {
            public Account Account { get; set; }
            public Brand Brand { get; set; }
            public ProdWithCat[] ProdWithCats { get; set; }
            public Command(Account account, Brand brand, ProdWithCat[] prodWithCats)
            {
                Account = account;
                Brand = brand;
                ProdWithCats = prodWithCats;
            }
        }


        public class Handaler : IRequestHandler<Command, int>
        {
            private readonly IBrandRepository _brandRepository;
            public Handaler(IBrandRepository brandRepository)
            {
                _brandRepository=brandRepository;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _brandRepository.InsertWithProducts(request.Account, request.Brand, request.ProdWithCats);
            }
        }
    }
}
