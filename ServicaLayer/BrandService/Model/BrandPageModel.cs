using System.Collections.Generic;

namespace ServicaLayer.BrandService.Model
{
    /// <summary>
    /// model used to create the object that will be returned in brand API brand/Page/{page}/{PageSize}
    /// </summary>
    public class BrandPageModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalBrand { get; set; }
        public IEnumerable<BrandForPage> Brands { get; set; }
        public int TotalPages { get; set; }

    }
    /// <summary>
    /// data of each brand needed in brands page
    /// </summary>
    public class BrandForPage
    {
        public int Id { get; set; }
        //public IEnumerable<ProductForBrandPage> IdProducts { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
    public class ProductForBrandPage
    {
        public int Id { get; set; }
    }
}
