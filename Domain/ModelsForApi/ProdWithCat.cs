using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelsForApi
{
    
        public class ProdWithCat
        {
            public Product Product { get; set; }
            public int[] CategoriesIds { get; set; }
        }
    
}
