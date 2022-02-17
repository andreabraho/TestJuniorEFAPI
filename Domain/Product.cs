using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// rappresent a product of the application
    /// </summary>
    public class Product:EntityBase
    {
        private string _name;
        public string Name { get { return _name; }
            set { /*ValidateGenericString(value);*/_name = value; } }
        private string _shortDescription;
        public string ShortDescription { get { return _shortDescription; }
            set { ValidateGenericString(value);_shortDescription = value; } }
        public string Description { get; set; }
        private decimal _price;
        public decimal Price { get { return _price; }
            set { ValidatePrice(value);_price = value; } }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<InfoRequest> InfoRequests { get; set; }
        public string GetFakeImage() => "https://via.placeholder.com/728x90.png?text="+ShortDescription.Substring(0, ShortDescription.Length<10?ShortDescription.Length:10);
        private void ValidateGenericString(string str)
        {
            if(string.IsNullOrWhiteSpace(str) || str.Length > 255)
            {
                throw new ArgumentException(nameof(str));
            }
        }
        private void ValidatePrice(decimal price)
        {
            if(price < 0 || price >(decimal)(1e16))
                throw new ArgumentException(nameof(price));
        }
    
    }
}
