using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicaLayer.ProductService.Model
{
    public class GetUpdateProductDTO
    {
        public Product Product { get; set; }
        public int[] CategoriesAssociated { get; set; }
        public IEnumerable<BrandForInsertDTO> AllBrands { get; set; }
        public IEnumerable<CatForInsertDTO> AllCategories { get; set; }
    }
}
