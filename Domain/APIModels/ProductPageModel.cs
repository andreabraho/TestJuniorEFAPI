using System.Collections.Generic;

namespace Domain.APIModels
{
    /// <summary>
    /// model used to create the object that will be returned in Produc API product/Page/{page}{PageSize}
    /// </summary>
    public class ProductPageModel
    {
        /// <summary>
        /// page needed
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// number of products for each page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// total number of products
        /// </summary>
        public int TotalProducts { get; set; }
        public int TotalPages { get; set; }
        /// <summary>
        /// list of products for the page
        /// </summary>
        public IEnumerable<ProductForPage> Products { get; set; } 
    }
    /// <summary>
    /// data needed for each product for the product paging api
    /// </summary>
    public class ProductForPage
    {
        public int Id { get; set; }
        /// <summary>
        /// product name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// product image link
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// product short description
        /// </summary>
        public string ShortDescription { get; set; }
    }
}
