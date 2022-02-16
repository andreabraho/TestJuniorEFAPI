using DataLayer.Interfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Commands
{
    public static class UpsertProduct
    {
        public class Command: IRequest<Response>
        {
            public Product Product { get; set; }
            public int[] CategoriesIds { get; set; }
            public Command(Product product,int[] categoriesIds)
            {
                Product = product;
                CategoriesIds = categoriesIds;
            }

        }

        public class Handaler : IRequestHandler<Command, Response>
        {
            private readonly IProductRepository _productRepository;
            public Handaler(IProductRepository productRepository)
            {
                _productRepository= productRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var result=await _productRepository.Upsert(request.Product,request.CategoriesIds);
                return new Response { Id=result};
            }
        }






        public class Response
        {
            public int Id { get; set; }
        }
    }
}
