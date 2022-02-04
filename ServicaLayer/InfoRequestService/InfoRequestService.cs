using DataLayer.Interfaces;
using DataLayer.QueryObjects;
using Domain;
using Microsoft.EntityFrameworkCore;
using ServicaLayer.InfoRequestService.Model;
using ServicaLayer.InfoRequestService.QueryObjects;
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
        public InfoRequestService(IRepository<InfoRequest> infoRequestRepository)
        {
            _infoRequestRepository=infoRequestRepository;
        }
        public InfoRequestPageDTO GetPage(int page, int pageSize,int idBrand=0,string productNameSearch=null,bool isAsc=true)
        {

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            var pageModel = new InfoRequestPageDTO
            {
                Page = page,
                PageSize = pageSize,
                TotalinfoRequests = _infoRequestRepository.GetAll().Count(),

            };

            var query = _infoRequestRepository.GetAll();

            query = query.FilterIR(idBrand, productNameSearch);
           
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
                Name = ir.Name,
                LastName = ir.LastName,
                Email = ir.Email,
                Location = ir.City + "(" + ir.Cap + "), " + ir.Nation.Name,
                IRModelReplies = ir.InfoRequestReplys.OrderByDescending(x => x.InsertDate).Select(r => new IRModelReplyDTO
                {
                    Id = r.Id,
                    ReplyText = r.ReplyText,
                    User = r.Account.AccountType == 1 ? r.Account.Brand.BrandName : ir.Name + " " + ir.LastName,
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















        //test not working ---------------------------------------------------------------------------------------------------
        public InfoRequestDetailDTO GetInfoRequestDetail2(int id)//in progress
        {
            if (id <= 0)
                throw new ArgumentException(nameof(id));

            var query = _infoRequestRepository.GetById(id).MapIRForDetailPage();

            return query.FirstOrDefault();
        }
    }
}
