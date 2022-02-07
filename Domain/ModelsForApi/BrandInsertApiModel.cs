using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ModelsForApi
{
    public class BrandInsertApiModel
    {
        public Account Account { get; set; }
        public Brand Brand { get; set; }
        public ProdWithCat[] prodsWithCats { get; set; }
    }
}
