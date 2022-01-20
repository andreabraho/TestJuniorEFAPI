using System.Collections.Generic;

namespace Domain.APIModels
{
   /// <summary>
   /// model used to create the object that will be returned in Produc API Page/{page}{PageSize}
   /// </summary>
    public class ProductPageModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalProducts { get; set; }
        public List<ProductForPage> Products { get; set; }=new List<ProductForPage> { };
    }

    public class ProductForPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
    }
}
