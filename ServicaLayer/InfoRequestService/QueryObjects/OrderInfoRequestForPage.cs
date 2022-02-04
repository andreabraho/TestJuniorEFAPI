using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.InfoRequestService.QueryObjects
{
    public static class OrderInfoRequestForPage
    {
        public static IQueryable<InfoRequest> OrderInfoRequest(this IQueryable<InfoRequest> infoRequests,bool isAsc)
        {
            if (isAsc)
                infoRequests = infoRequests.OrderBy(x => x.InsertDate);
            else
                infoRequests = infoRequests.OrderByDescending(x => x.InsertDate);
            return infoRequests;
        }
    }
}
