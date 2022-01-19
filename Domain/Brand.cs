using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Brand:EntityBase
    {
        public string BrandName { get; set; }
        public string Description   { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public ICollection<Product> Products { get; set; }  
    }
}
