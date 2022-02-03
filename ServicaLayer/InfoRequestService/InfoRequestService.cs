using DataLayer.Interfaces;
using Domain;
using Domain.APIModels;
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
        public IQueryable GetPage()
        {
            var t = _infoRequestRepository.GetAll().Select(ir => new
            {
                ir.Id,
                User = new
                {
                    ir.Name,
                    ir.LastName,
                    ir.PhoneNumber,
                    ir.Email,
                    ir.City,
                    ir.Cap,
                },
                ir.RequestText,
                ir.InsertDate,
                ir.ProductId,
                ProductName = ir.Product.Name,
                BrandId = ir.Product.BrandId,
                BrandName = ir.Product.Brand.BrandName
            });

            return t;
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
        public InfoRequestDetailModel GetInfoRequestDetail2(int id)//in progress
        {
            if (id <= 0)
                throw new ArgumentException(nameof(id));

            var query = _infoRequestRepository.GetById(id).MapIRForDetailPage();

            return query.FirstOrDefault();
        }
    }
}
