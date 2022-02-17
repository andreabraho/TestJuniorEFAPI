using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CqrsServices.Queries.GetProductPage;

namespace CqrsServices.Queries.BrandQueries
{
    public static class GetBrandDetailById
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

            private readonly IBrandRepository _brandRepository;
            private readonly IRepository<Category> _categoryRepository;
            public Handaler(IBrandRepository brandRepository, IRepository<Category> categoryRepository)
            {
                _brandRepository = brandRepository;
                _categoryRepository = categoryRepository;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _brandRepository.GetById(request.Id).Select(b => new Response
                {
                    Id = b.Id,
                    Name = b.BrandName,
                    CountRequestFromBrandProducts = b.Products.SelectMany(x => x.InfoRequests).Count(),
                    TotProducts = b.Products.Count(),
                    Products = b.Products.Select(product => new ProductBrandDetailDTO
                    {
                        Id = product.Id,
                        CountInfoRequest = product.InfoRequests.Count(),
                        Name = product.Name,

                    }),
                    AssociatedCategory = _categoryRepository.GetAll().Where(x => b.Products.SelectMany(c => c.ProductCategories).Select(b => b.CategoryId).Contains(x.Id)).Select(ca => new CategoryBrandDetailDTO
                    {
                        Id = ca.Id,
                        Name = ca.Name,
                        CountProdAssociatied = b.Products.SelectMany(x => x.ProductCategories).Where(d => d.CategoryId == ca.Id).Count(),
                    }),

                });

                return await query.FirstOrDefaultAsync();
            }
        }



        /// <summary>
        /// Rappresents a class that contains all data neccessary for a brand detail page
        /// model used to create the object that will be returned in Produc API brand/Detail/{id}
        /// </summary>
        public class Response:CQRSResponse
        {

            /// <summary>
            /// brand id
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// brand name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// total of products inserted by the brand
            /// </summary>
            public int TotProducts { get; set; }
            /// <summary>
            /// total of all info requests recived by the brand
            /// </summary>
            public int CountRequestFromBrandProducts { get; set; }
            /// <summary>
            /// list of categories associated to the brand
            /// </summary>
            public IEnumerable<CategoryBrandDetailDTO> AssociatedCategory { get; set; }
            /// <summary>
            /// list of products associated to the brand
            /// </summary>
            public IEnumerable<ProductBrandDetailDTO> Products { get; set; }
        }
        /// <summary>
        /// data neccessary on the page fo each category
        /// </summary>
        public class CategoryBrandDetailDTO
        {
            /// <summary>
            /// category id
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// category name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// number of products associated to the category of the brand
            /// </summary>
            public int CountProdAssociatied { get; set; }
        }
        /// <summary>
        /// data neccessary on the page fo each product
        /// </summary>
        public class ProductBrandDetailDTO
        {
            /// <summary>
            /// id product
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// product name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// number of info requests recived by the product
            /// </summary>
            public int CountInfoRequest { get; set; }
        }




    }
}
