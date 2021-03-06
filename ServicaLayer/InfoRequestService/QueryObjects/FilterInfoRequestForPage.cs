using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicaLayer.InfoRequestService.QueryObjects
{
    public static class FilterInfoRequestForPage
    {
        public static IQueryable<InfoRequest> FilterIR(this IQueryable<InfoRequest> infoRequests, int idBrand = 0, string productNameSearch = null,int productId=0)
        {

            if (idBrand != 0)
                infoRequests= infoRequests.Where(x => x.Product.BrandId == idBrand);
            if (productNameSearch != null)
                infoRequests= infoRequests.Where(x => x.Product.Name.Contains(productNameSearch));
            if(productId > 0)
                infoRequests = infoRequests.Where(x => x.Product.Id==productId);
            return infoRequests;

        }
    }
}
