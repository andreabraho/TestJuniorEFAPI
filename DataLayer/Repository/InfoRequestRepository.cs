using DataLayer.Interfaces;
using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repository
{
    public class InfoRequestRepository : Repository<InfoRequest>, IInfoRequestRepository
    {
        public InfoRequestRepository(MyContext context) : base(context) { }
        public InfoRequestDetailModel InfoRequestDetail(int id)
        {
            return null;
        }

    }
}
