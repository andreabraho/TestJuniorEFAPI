using DataLayer.Interfaces;
using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repository
{
    public class InfoRequestRepository : Repository<InfoRequest>, IInfoRequestRepository
    {
        public InfoRequestRepository(MyContext context) : base(context) { }
        /// <summary>
        /// all neccessary data for a info request detail page
        /// </summary>
        /// <param name="id">id of the info request</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id <=0</exception>
        public InfoRequestDetailModel InfoRequestDetail(int id)
        {
            if(id<=0)
                throw new ArgumentException(nameof(id));
            InfoRequestDetailModel x = _ctx.InfoRequests
                    .Where(x => x.Id == id)
                    .Select(ir => new InfoRequestDetailModel
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
                        IRModelReplies = ir.InfoRequestReplys.OrderByDescending(x=>x.InsertDate).Select(r => new IRModelReply
                        {
                            Id=r.Id,
                            ReplyText = r.ReplyText,
                            User=r.Account.AccountType==1?r.Account.Brand.BrandName:ir.Name+" "+ir.LastName,
                        }).ToList(),
                    }).FirstOrDefault(x=>x.Id==id);

            return x;
        }
        /// <summary>
        /// all neccessary data for a info request detail page
        /// </summary>
        /// <param name="id">id of the info request</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id <=0</exception>
        public InfoRequestDetailModel InfoRequestDetailV2(int id)
        {
            //InfoRequestDetailModel x = _ctx.InfoRequests
            //        .Where(x => x.Id == id)
            //        .Select(ir => new InfoRequestDetailModel
            //        {
            //            Id = ir.Id,
            //            productIRDetail = new ProductIRDetail
            //            {
            //                Id = ir.Product.Id,
            //                BrandName = ir.Product.Brand.BrandName,
            //                Name = ir.Product.Name,
            //            },
            //            Name = ir.Name,
            //            LastName = ir.LastName,
            //            Email = ir.Email,
            //            Location = ir.City + "(" + ir.Cap + "), " + ir.Nation.Name,
            //            IRModelReplies = ir.InfoRequestReplys.OrderByDescending(x => x.InsertDate).Select(r => new IRModelReply
            //            {
            //                Id = r.Id,
            //                ReplyText = r.ReplyText,
            //                User = r.Account.AccountType == 1 ? r.Account.Brand.BrandName : ir.Name + " " + ir.LastName,
            //            }).ToList(),
            //        }).FirstOrDefault(x => x.Id == id);


            //var query=_ctx.InfoRequests.AsQueryable();
            //query=query.Where(x => x.Id == id);

            var query = _ctx.InfoRequests.AsQueryable();

            var infoRequestDetailModel =
                from InfoRequests in query
                let irr = InfoRequests.InfoRequestReplys
                let p=InfoRequests.Product
                let b = p.Brand
                let n=InfoRequests.Nation
                //from InfoRequestReply in ir.InfoRequestReplys
                where InfoRequests.Id == id
                select new InfoRequestDetailModel
                {
                    Id = InfoRequests.Id,
                    Name = InfoRequests.Name,
                    LastName = InfoRequests.LastName,
                    Email = InfoRequests.Email,
                    Location = InfoRequests.City + "(" + InfoRequests.Cap + "), " + n.Name,
                    productIRDetail = new ProductIRDetail
                    {
                        Id = p.Id,
                        BrandName = b.BrandName,
                        Name = p.Name,
                    },
                    IRModelReplies = irr.OrderByDescending(x => x.InsertDate).Select(r => new IRModelReply
                    {
                        Id = r.Id,
                        ReplyText = r.ReplyText,
                        User = r.Account.AccountType == 1 ? b.BrandName : InfoRequests.Name + " " + InfoRequests.LastName,
                    }).ToList(),
                };

            return infoRequestDetailModel.FirstOrDefault();
            
        }

    }
}
