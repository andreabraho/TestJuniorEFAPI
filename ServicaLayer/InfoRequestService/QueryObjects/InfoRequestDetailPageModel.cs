using Domain;
using ServicaLayer.InfoRequestService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.InfoRequestService.QueryObjects
{
    public static class InfoRequestDetailPageModel
    {
        //TODO NON RIESCO AD USARLO WHY???
        public static IQueryable<InfoRequestDetailDTO> MapIRForDetailPage(this IQueryable<InfoRequest> infoRequest)
        {
            return infoRequest.Select(ir => new InfoRequestDetailDTO
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
                IRModelReplies = ir.InfoRequestReplys.OrderByDescending(x => x.InsertDate).AsQueryable().MapInfoReplysForIRDetail(ir),
            });
        }
        public static IQueryable<IRModelReplyDTO> MapInfoReplysForIRDetail(this IQueryable<InfoRequestReply> irReplys,InfoRequest ir)
        {
            return irReplys.Select(r => new IRModelReplyDTO
            {
                Id = r.Id,
                ReplyText = r.ReplyText,
                User = r.Account.AccountType == 1 ? r.Account.Brand.BrandName : ir.Name + " " + ir.LastName,
            });
        }
    }
}
