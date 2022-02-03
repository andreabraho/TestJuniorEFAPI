using DataLayer.Interfaces;
using Domain;
using ServicaLayer.InfoRequestService.Model;
using ServicaLayer.InfoRequestService.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ServicaLayer.InfoRequestService
{
    public class InfoRequestService
    {
        IRepository<InfoRequest> _infoRequestRepository;
        public InfoRequestService(IRepository<InfoRequest> infoRequestRepository)
        {
            _infoRequestRepository=infoRequestRepository;
        }
        public InfoRequestPageModel GetPage(int page, int pageSize,int idBrand=0,string productNameSearch=null,bool isAsc=true)
        {

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            var pageModel = new InfoRequestPageModel
            {
                Page = page,
                PageSize = pageSize,
                TotalinfoRequests = _infoRequestRepository.GetAll().Count(),

            };

            var query = _infoRequestRepository.GetAll();
            if(idBrand != 0)
                query = query.Where(x => x.Product.BrandId==idBrand);
            if(productNameSearch !=null)
                query = query.Where(x => x.Product.Name.Contains(productNameSearch));

            if (isAsc)
                query = query.OrderBy(x => x.InsertDate);
            else
                query = query.OrderByDescending(x => x.InsertDate);

            pageModel.infoRequests = query.Skip(pageSize * (page - 1)).Take(pageSize).MapIrForPaging();

            pageModel.TotalPages = CalculateTotalPages(pageModel.TotalinfoRequests, pageSize);

            return pageModel;
        }



        public InfoRequestDetailModel GetInfoRequestDetail(int id)
        {
            if (id <= 0)
                throw new ArgumentException(nameof(id));

            var query = _infoRequestRepository.GetById(id).Select(ir => new InfoRequestDetailModel
            {
                Id = ir.Id,
                productIRDetail = new ProductIRDetail
                {
                    Id = ir.Product.Id,
                    BrandName = ir.Product.Brand.BrandName,
                    Name = ir.Product.Name,
                },
                Name = ir.Name,
                LastName = ir.LastName,
                Email = ir.Email,
                Location = ir.City + "(" + ir.Cap + "), " + ir.Nation.Name,
                IRModelReplies = ir.InfoRequestReplys.OrderByDescending(x => x.InsertDate).Select(r => new IRModelReply
                {
                    Id = r.Id,
                    ReplyText = r.ReplyText,
                    User = r.Account.AccountType == 1 ? r.Account.Brand.BrandName : ir.Name + " " + ir.LastName,
                }),
            });

            return query.FirstOrDefault();
        }


        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (totalItems % pageSize == 0)
                return totalItems / pageSize;
            else
                return (totalItems / pageSize) + 1;
        }















        //test not working ---------------------------------------------------------------------------------------------------
        public InfoRequestDetailModel GetInfoRequestDetail2(int id)//in progress
        {
            if (id <= 0)
                throw new ArgumentException(nameof(id));

            var query = _infoRequestRepository.GetById(id).MapIRForDetailPage();

            return query.FirstOrDefault();
        }
    }
}
