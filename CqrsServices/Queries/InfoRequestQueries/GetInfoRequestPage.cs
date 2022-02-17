using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Queries.InfoRequestQueries
{
    public static class GetInfoRequestPage
    {
        public class Query : IRequest<Response>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int BrandId { get; set; }
            public string ProdNameSearch { get; set; }
            public bool IsAsc { get; set; }
            public int ProductId { get; set; }

            public Query(int page, int pageSize, int brandId, string prodNameSearch, bool isAsc, int productId)
            {
                Page = page;
                PageSize = pageSize;
                BrandId = brandId;
                ProdNameSearch = prodNameSearch;
                IsAsc = isAsc;
                ProductId = productId;
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
                var response = new Response
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalinfoRequests = _infoRequestRepository.GetAll().FilterIR(request.BrandId, request.ProdNameSearch, request.ProductId).Count(),
                    Brands = _brandRepository.GetAll().Select(b => new BrandForPageDTO
                    {
                        Id = b.Id,
                        Name = b.BrandName
                    })
                };

                var query = _infoRequestRepository.GetAll();

                query = query.FilterIR(request.BrandId, request.ProdNameSearch, request.ProductId);

                query = query.OrderInfoRequest(request.IsAsc);

                query = query.Page(request.Page, request.PageSize);

                response.infoRequests = query.MapIrForPaging();

                response.TotalPages = CalculateTotalPages(response.TotalinfoRequests, request.PageSize);

                return response;
            }
        }

        public class Response
        {
            /// <summary>
            /// page needed
            /// </summary>
            public int Page { get; set; }
            /// <summary>
            /// number of info requests for each page
            /// </summary>
            public int PageSize { get; set; }
            /// <summary>
            /// total number of info requests
            /// </summary>
            public int TotalinfoRequests { get; set; }
            public int TotalPages { get; set; }

            public IEnumerable<IRForPageModel> infoRequests { get; set; }
            public IEnumerable<BrandForPageDTO> Brands { get; set; }



        }
        public class IRForPageModel
        {
            public int Id { get; set; }
            public string RequestText { get; set; }
            public DateTime InsertDate { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int BrandId { get; set; }
            public string BrandName { get; set; }

            public UserModelForIR User { get; set; }

        }
        public class UserModelForIR
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string City { get; set; }
            public string Cap { get; set; }

        }
        public class BrandForPageDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public static IQueryable<InfoRequest> FilterIR(this IQueryable<InfoRequest> infoRequests, int idBrand = 0, string productNameSearch = null, int productId = 0)
        {

            if (idBrand != 0)
                infoRequests = infoRequests.Where(x => x.Product.BrandId == idBrand);
            if (productNameSearch != null)
                infoRequests = infoRequests.Where(x => x.Product.Name.Contains(productNameSearch));
            if (productId > 0)
                infoRequests = infoRequests.Where(x => x.Product.Id == productId);
            return infoRequests;

        }
        public static IQueryable<InfoRequest> OrderInfoRequest(this IQueryable<InfoRequest> infoRequests, bool isAsc)
        {
            if (isAsc)
                infoRequests = infoRequests.OrderBy(x => x.InsertDate);
            else
                infoRequests = infoRequests.OrderByDescending(x => x.InsertDate);
            return infoRequests;
        }
        public static IQueryable<IRForPageModel> MapIrForPaging(this IQueryable<InfoRequest> infoRequest)
        {
            return infoRequest.Select(ir => new IRForPageModel
            {
                Id = ir.Id,
                User = new UserModelForIR
                {
                    Name = ir.Name,
                    LastName = ir.LastName,
                    PhoneNumber = ir.PhoneNumber,
                    Email = ir.Email,
                    City = ir.City,
                    Cap = ir.Cap,
                },
                RequestText = ir.RequestText,
                InsertDate = ir.InsertDate,
                ProductId = ir.ProductId,
                ProductName = ir.Product.Name,
                BrandId = ir.Product.BrandId,
                BrandName = ir.Product.Brand.BrandName
            });
        }
        /// <summary>
        /// calculate how many pages can be
        /// </summary>
        /// <param name="totalItems"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private static int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }
    }
}
