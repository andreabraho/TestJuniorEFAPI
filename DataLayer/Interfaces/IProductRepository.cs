using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        ///// <summary>
        ///// data for a product detail page
        ///// </summary>
        ///// <param name="id">id of the product</param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentOutOfRangeException">if id <=0 </exception>
        //public ProductDetailModel GetProductDetail(int id);
        ///// <summary>
        ///// data for a product detail page
        ///// </summary>
        ///// <param name="id">id of the product</param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentOutOfRangeException">if id <=0 </exception>
        //public ProductDetailModel GetProductDetailV2(int id);
        ///// <summary>
        ///// get products for a page
        ///// </summary>
        ///// <param name="page">page wanted</param>
        ///// <param name="pageSize">page dimension</param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentOutOfRangeException">in case page or pageSize<=0</exception>
        //public List<Product> GetPageProducts(int page,int pageSize);
        //public int GetCount();
    }
}
