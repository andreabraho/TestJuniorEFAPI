using System.Collections.Generic;

namespace ServicaLayer.BrandService.Model
{
    /// <summary>
    /// Rappresents a class that contains all data neccessary for a brand detail page
    /// model used to create the object that will be returned in Produc API brand/Detail/{id}
    /// </summary>
    public class BrandDetailDTO
    {

        /// <summary>
        /// brand id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// brand name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// total of products inserted by the brand
        /// </summary>
        public int TotProducts { get; set; }
        /// <summary>
        /// total of all info requests recived by the brand
        /// </summary>
        public int CountRequestFromBrandProducts { get; set; }
        /// <summary>
        /// list of categories associated to the brand
        /// </summary>
        public IEnumerable<CategoryBrandDetailDTO> AssociatedCategory { get; set; }
        /// <summary>
        /// list of products associated to the brand
        /// </summary>
        public IEnumerable<ProductBrandDetailDTO> Products { get; set; }
    }
    /// <summary>
    /// data neccessary on the page fo each category
    /// </summary>
    public class CategoryBrandDetailDTO
    {
        /// <summary>
        /// category id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// category name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// number of products associated to the category of the brand
        /// </summary>
        public int CountProdAssociatied { get; set; }
    }
    /// <summary>
    /// data neccessary on the page fo each product
    /// </summary>
    public class ProductBrandDetailDTO
    {
        /// <summary>
        /// id product
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// number of info requests recived by the product
        /// </summary>
        public int CountInfoRequest { get; set; }
    }
}
