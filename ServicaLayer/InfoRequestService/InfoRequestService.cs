using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using Microsoft.EntityFrameworkCore;
using ServicaLayer.InfoRequestService.Model;
using ServicaLayer.InfoRequestService.QueryObjects;
using ServicaLayer.ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicaLayer.InfoRequestService
{
    public class InfoRequestService
    {
        IRepository<InfoRequest> _infoRequestRepository;
        IRepository<Brand> _brandRepository;
        public InfoRequestService(IRepository<InfoRequest> infoRequestRepository, IRepository<Brand> brandRepository)
        {
            _infoRequestRepository=infoRequestRepository;
            _brandRepository = brandRepository;
        }
        public InfoRequestPageDTO GetPage(int page, int pageSize,int idBrand=0,string productNameSearch=null,bool isAsc=true,int productId=0)
        {

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            var pageModel = new InfoRequestPageDTO
            {
                Page = page,
                PageSize = pageSize,
                TotalinfoRequests = _infoRequestRepository.GetAll().FilterIR(idBrand, productNameSearch,productId).Count(),
                Brands = _brandRepository.GetAll().Select(b => new BrandForPageDTO
                {
                    Id = b.Id,
                    Name = b.BrandName
                })
            };

            var query = _infoRequestRepository.GetAll();

            query = query.FilterIR(idBrand, productNameSearch,productId);
           
            query = query.OrderInfoRequest(isAsc);

            query=query.Page(page,pageSize);

            pageModel.infoRequests = query.MapIrForPaging();

            pageModel.TotalPages = CalculateTotalPages(pageModel.TotalinfoRequests, pageSize);

            return pageModel;
        }



        async public Task<InfoRequestDetailDTO> GetInfoRequestDetail(int id)
        {
            if (id <= 0)
                throw new ArgumentException(nameof(id));

            var query = _infoRequestRepository.GetById(id).Select(ir => new InfoRequestDetailDTO
            {
                Id = ir.Id,
                productIRDetail = new ProductIRDetailDTO
                {
                    Id = ir.Product.Id,
                    BrandName = ir.Product.Brand.BrandName,
                    Name = ir.Product.Name,
                },
                RequestText=ir.RequestText,
                Name = ir.Name,
                LastName = ir.LastName,
                Email = ir.Email,
                Location = ir.City + "(" + ir.Cap + "), " + ir.Nation.Name,
                IRModelReplies = ir.InfoRequestReplys.OrderByDescending(x => x.InsertDate).Select(r => new IRModelReplyDTO
                {
                    Id = r.Id,
                    ReplyText = r.ReplyText,
                    User = r.Account.AccountType == 1 ? r.Account.Brand.BrandName : ir.Name + " " + ir.LastName,
                    Date=r.InsertDate,
                }),
            });

            return await query.FirstOrDefaultAsync();
        }


        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }

    }
}
