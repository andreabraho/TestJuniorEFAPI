using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Category:EntityBase
    {
        public string Name { get; set; }   
        public IEnumerable<ProductCategory> Product_Categories { get; set; }
        
    }
}
