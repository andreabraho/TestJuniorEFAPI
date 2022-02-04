using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.InfoRequestService.QueryObjects
{
    public static class PagingInfoRequests
    {
        public static IQueryable<InfoRequest> PageInfoRequests(this IQueryable<InfoRequest> infoRequests,int page,int pageSize)
        {
            return infoRequests.Skip(pageSize * (page - 1)).Take(pageSize);
        }
    }
}
