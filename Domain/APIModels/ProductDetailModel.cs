using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.APIModels
{
    public class ProductDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public List<Category> productsCategory  { get; set; }
        public int countGuestInfoRequests { get; set; }
        public int countUserInfoRequests { get; set; }
        public List<InfoRequestProductModel> infoRequestProducts { get; set; } = new List<InfoRequestProductModel>();

    }
    public class InfoRequestProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int ReplyNumber { get; set; }
        public DateTime DateLastReply   { get; set; }
    }
}
