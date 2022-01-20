using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Product:EntityBase
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Product_Category> Product_Categories { get; set; }
        public ICollection<InfoRequest> InfoRequests { get; set; }
        public string GetFakeImage() => "https://via.placeholder.com/728x90.png?text="+ShortDescription.Substring(0, 20);
    }
}
