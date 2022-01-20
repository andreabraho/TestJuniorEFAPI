using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface IInfoRequestRepository: IRepository<InfoRequest>
    {
        public InfoRequestDetailModel InfoRequestDetail(int id);

    }
}
