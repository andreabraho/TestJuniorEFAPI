using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Category:EntityBase
    {
        public string Name { get; set; }   
        public ICollection<Product_Category> Product_Categories { get; set; }
        
    }
}
