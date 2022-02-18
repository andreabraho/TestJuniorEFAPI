using CqrsServices.Validation;
using DataLayer.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Queries
{
    public static class GetProductDetailById
    {

        //query /command all data we need to execute
        public class Query:IRequest<Response>
        {
            
            public int Id { get; }  
            public Query(int id)
            {
                Id = id;
            }
        }

        public class Validator : IValidationHandler<Query>
        {
            public async Task<ValidationResult> Validate(Query request)
            {
                if (request.Id <= 0)
                    return ValidationResult.Fail("Id Requerst can't be lower of equal than 0");
                
                return ValidationResult.Success;
            }
        }

        //handaler all business logic added to execute,return a response

        public class Handaler : IRequestHandler<Query, Response>
        {
            private readonly IProductRepository _productRepository;
            public Handaler(IProductRepository productRepository)
            {
                _productRepository=productRepository;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)//
            {
                var query = _productRepository.GetById(request.Id).Select(p => new Response
                {
                    Id = p.Id,
                    Name = p.Name,
                    BrandId = p.Brand.Id,
                    BrandName = p.Brand.BrandName,

                    productsCategory = p.ProductCategories.Select(c => new CategoryProductDTO
                    {
                        Id = c.CategoryId,
                        Name = c.Category.Name,
                    }),
                    countGuestInfoRequests = p.InfoRequests.Where(x => x.UserId == null).Count(),
                    countUserInfoRequests = p.InfoRequests.Where(x => x.UserId != null).Count(),
                    infoRequestProducts = p.InfoRequests.OrderByDescending(x => x.InsertDate).Select(ir => new InfoRequestProductDTO
                    {
                        Id = ir.Id,
                        ReplyNumber = ir.InfoRequestReplys.Count(),
                        Name = ir.UserId == null ? ir.Name : ir.User.Name,
                        LastName = ir.UserId == null ? ir.LastName : ir.User.LastName,
                        DateLastReply = ir.InfoRequestReplys.Max(x => x.InsertDate),
                    }),
                });
                var response = await query.FirstOrDefaultAsync();
                return response;
            }
        }






        //response data we want to return
        public class Response:CQRSResponse
        {
            public int Id { get; set; }
            public int BrandId { get; set; }
            public string Name { get; set; }
            public string BrandName { get; set; }
            public IEnumerable<CategoryProductDTO> productsCategory { get; set; }
            public int countGuestInfoRequests { get; set; }
            public int countUserInfoRequests { get; set; }
            public IEnumerable<InfoRequestProductDTO> infoRequestProducts { get; set; }

        }
        public class InfoRequestProductDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public int ReplyNumber { get; set; }
            public DateTime DateLastReply { get; set; }
        }
        public class CategoryProductDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }








    }
}
