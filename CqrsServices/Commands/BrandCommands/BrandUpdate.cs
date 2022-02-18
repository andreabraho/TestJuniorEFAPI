using CqrsServices.Validation;
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
        public class Command : IRequest<Response>
        {
            public Brand Brand { get; set; }
            public Command(Brand brand)
            {
                Brand=brand;
            }
            
        }

        public class Validator : IValidationHandler<Command>
        {
            public async Task<ValidationResult> Validate(Command request)
            {
                if (request.Brand == null)
                    return ValidationResult.Fail("Brand can't be null");

                var result=ValidateBrandUpdate(request.Brand);
                if(result!=null)
                    return ValidationResult.Fail(result);
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
                Brand BrandFromRepo = await _brandRepository.GetById(request.Brand.Id).FirstOrDefaultAsync();
                if (BrandFromRepo == null)
                    throw new NullReferenceException("id not valid");

                BrandFromRepo.BrandName = request.Brand.BrandName;
                BrandFromRepo.Description = request.Brand.Description;
                if (await _brandRepository.Update(BrandFromRepo) > 0)
                    return new Response { Result = true,BrandId=BrandFromRepo.Id };
                else
                    return new Response { Result = false };
            }
        }
        public class Response : CQRSResponse
        {
            public bool Result { get; set; }
            public int BrandId { get; set; }

        }

        /// <summary>
        /// validates the model in input for brand Update api
        /// </summary>
        /// <param name="brand"></param>
        /// <returns>null if the model is valid,
        /// string with error if not</returns>
        private static string ValidateBrandUpdate(Brand brand)
        {
            string result = null;
            if (string.IsNullOrWhiteSpace(brand.BrandName))
            {
                result = "Brand name can't be null o empity";
            }else
            if (brand.BrandName.Length > 255)
                result = "Not valid Brand Name can't have more than 255 characters";

            return result;
        }
    }
}
