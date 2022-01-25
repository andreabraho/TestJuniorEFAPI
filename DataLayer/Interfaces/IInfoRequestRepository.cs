using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface IInfoRequestRepository: IRepository<InfoRequest>
    {
        /// <summary>
        /// all neccessary data for a info request detail page
        /// </summary>
        /// <param name="id">id of the info request</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id <=0</exception>
        public InfoRequestDetailModel InfoRequestDetail(int id);
        /// <summary>
        /// all neccessary data for a info request detail page
        /// </summary>
        /// <param name="id">id of the info request</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id <=0</exception>
        public IQueryable<InfoRequestDetailModel> InfoRequestDetailV2(int id);
    }
}
