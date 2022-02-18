using CqrsServices.Validation;
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
        public class Command : IRequest<Response>
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
        public class Validator : IValidationHandler<Command>
        {
            public async Task<ValidationResult> Validate(Command request)
            {
                if (request.Account == null)
                    return ValidationResult.Fail("Account can't be null");
                if (request.Brand == null)
                    return ValidationResult.Fail("Brand can't be null");
                if (request.ProdWithCats == null)
                    return ValidationResult.Fail("Products can't be null");
                var result = ValidateBrandInsert(request.Account, request.Brand, request.ProdWithCats);
                if (result != null)
                    return ValidationResult.Fail(result);

                return ValidationResult.Success;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IBrandRepository _brandRepository;
            public Handler(IBrandRepository brandRepository)
            {
                _brandRepository=brandRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                return new Response { Id=await _brandRepository.InsertWithProducts(request.Account, request.Brand, request.ProdWithCats) };
            }
        }

        public class Response : CQRSResponse
        {
            public int Id { get; set; } 
        }





        /// <summary>
        /// validates data for brand insert
        /// </summary>
        /// <param name="brandInsertApiModel"></param>
        /// <returns>null if the model is valid,
        /// string with error if not</returns>
        private static string ValidateBrandInsert(Account account, Brand brand, ProdWithCat[] prodWithCats)
        {
            string result = null;
            if (string.IsNullOrWhiteSpace(account.Email))
            {
                result += "Email can't be empity or null";
                
            }
            else
            {
                if (account.Email.Length > 255)
                    result += "Email can't have more than 255 charaters \n";
            }
            
            if (string.IsNullOrWhiteSpace(account.Password))
            {
                result += "Password can't be empity or null";
            }
            else
            {
                if (account.Password.Length > 18)
                    result += "Password can't have more than 18 characters \n";
            }
            
            if (string.IsNullOrWhiteSpace(brand.BrandName))
            {
                result += "Brand name can't be empity or null";
            }
            else
            {
                if (brand.BrandName.Length > 255)
                    result = "Brand name can't have more than 255 characters \n";
            }

            if (!IsValidEmail(account.Email))
                result += "Email pattern is not valid";

            foreach (ProdWithCat prod in prodWithCats)
            {
                if (prod.CategoriesIds.Length == 0)
                {
                    result += "Select at least one category for each product \n";
                    break;
                }
            }
                

            foreach (ProdWithCat prod in prodWithCats)
            {
                if (string.IsNullOrWhiteSpace(prod.Product.Name))
                {
                    result += "Products name can't be empity or null";
                    break;
                }
                if (prod.Product.Name.Length > 255)
                {
                    result += "Product names can't have more than 255 characters \n";
                    break;
                }
            }
            foreach (ProdWithCat prod in prodWithCats)
            {
                if (string.IsNullOrWhiteSpace(prod.Product.ShortDescription))
                {
                    result += "Products shor description can't be null or empity";
                }
                if (prod.Product.ShortDescription.Length > 0)
                {
                    result += "Products short description can't have more than 255 characters\n";
                    break;
                }
            }
            foreach (ProdWithCat prod in prodWithCats)
            {
                if (prod.Product.Price < 0 || prod.Product.Price > (decimal)1e16)
                {
                    result += "price can't be lower than 0 or higher than 1e16 \n";
                    break;
                }
            }

            return result;
        }
        private static bool IsValidEmail(string email)
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
}
