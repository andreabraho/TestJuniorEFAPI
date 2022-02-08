using System;
using System.Collections.Generic;
using System.Text;

namespace ServicaLayer.ProductService.Model
{
    public class GetInsertProductDTO
    {
        public IEnumerable<BrandForInsertDTO> Brands { get; set; }
        public IEnumerable<CatForInsertDTO> Categories { get; set; }

    }
    public class BrandForInsertDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }   
    }
    public class CatForInsertDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
