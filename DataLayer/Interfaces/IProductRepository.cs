using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        public ProductDetailModel GetProductDetail(int id);
        public List<Product> GetPageProducts(int page,int pageSize);
        public int GetCount();
    }
}
