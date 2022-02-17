using CqrsServices.Validation;
using DataLayer.Interfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Commands.ProductCommands
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




        public class Validator : IValidationHandler<Command>
        {
            public async Task<ValidationResult> Validate(Command request)
            {
                var errMess= UpSertProductValidation(request.Product, request.CategoriesIds);
                if (errMess != null)
                    return ValidationResult.Fail(errMess);
                else
                    return ValidationResult.Success;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IProductRepository _productRepository;
            public Handler(IProductRepository productRepository)
            {
                _productRepository= productRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var result=await _productRepository.Upsert(request.Product,request.CategoriesIds);
                return new Response { Id=result};
            }
        }






        public class Response:CQRSResponse
        {
            public int Id { get; set; }
        }











        private static string UpSertProductValidation(Product product, int[] cats)
        {
            string result = null;

            if (cats.Length == 0)
            {
                result += "Select at least one category for the product \n";
            }
            if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Length == 0 || product.Name.Length > 255)
            {
                result += "Product name can't be empity and can't have more than 255 characters \n";
            }
            if (string.IsNullOrWhiteSpace(product.ShortDescription) || product.ShortDescription.Length == 0 || product.ShortDescription.Length > 255)
            {
                result += "Product short description can't be empity and can't have more than 255 characters \n";
            }
            if (product.Price < 0 || product.Price > (decimal)1e16)
            {
                result += "Price can't be lower than 0 or higher than 1e16";
            }
            if (product.BrandId < 0)
            {
                result += "Brand id can't be 0";
            }
            return result;
        }
    }
}
