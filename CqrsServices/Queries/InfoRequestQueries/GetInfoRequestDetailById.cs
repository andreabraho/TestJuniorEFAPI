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

namespace CqrsServices.Queries.InfoRequestQueries
{
    public static class GetInfoRequestDetailById
    {
        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
            public Query(int id)
            {
                Id = id;
            }
        }



        public class Handaler : IRequestHandler<Query, Response>
        {
            IRepository<InfoRequest> _infoRequestRepository;
            IRepository<Brand> _brandRepository;
            public Handaler(IRepository<InfoRequest> infoRequestRepository, IRepository<Brand> brandRepository)
            {
                _infoRequestRepository = infoRequestRepository;
                _brandRepository = brandRepository;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = _infoRequestRepository.GetById(request.Id).Select(ir => new Response
                {
                    Id = ir.Id,
                    productIRDetail = new ProductIRDetailDTO
                    {
                        Id = ir.Product.Id,
                        BrandName = ir.Product.Brand.BrandName,
                        Name = ir.Product.Name,
                    },
                    RequestText = ir.RequestText,
                    Name = ir.Name,
                    LastName = ir.LastName,
                    Email = ir.Email,
                    Location = ir.City + "(" + ir.Cap + "), " + ir.Nation.Name,
                    IRModelReplies = ir.InfoRequestReplys.OrderByDescending(x => x.InsertDate).Select(r => new IRModelReplyDTO
                    {
                        Id = r.Id,
                        ReplyText = r.ReplyText,
                        User = r.Account.AccountType == 1 ? r.Account.Brand.BrandName : ir.Name + " " + ir.LastName,
                        Date = r.InsertDate,
                    }),
                });

                return await response.FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// all data needed in info request detail page
        /// model used to create the object that will be returned in Produc API InfoRequest/Detail/{id}
        /// </summary>
        public class Response:CQRSResponse
        {
            /// <summary>
            /// info request id
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// all data of the product subject of the info request
            /// </summary>
            public ProductIRDetailDTO productIRDetail { get; set; }
            /// <summary>
            /// info request sender name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// info request sender lastName
            /// </summary>
            public string LastName { get; set; }
            /// <summary>
            /// info request sender email
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// string format containing “City (CAP), nation”
            /// </summary>
            public string Location { get; set; }
            /// <summary>
            /// list of all replies of the info request
            /// </summary>
            public string RequestText { get; set; }
            public IEnumerable<IRModelReplyDTO> IRModelReplies { get; set; }
        }
        /// <summary>
        /// product data needed for the info request detail page
        /// </summary>
        public class ProductIRDetailDTO
        {
            /// <summary>
            /// product id
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// product name
            /// </summary>
            public string Name { get; set; }
            public string BrandName { get; set; }

        }
        /// <summary>
        /// info request reply data needed for info request detail page
        /// </summary>
        public class IRModelReplyDTO
        {
            /// <summary>
            /// info request reply id
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// user sending the reply
            /// </summary>
            public string User { get; set; }
            public string ReplyText { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
