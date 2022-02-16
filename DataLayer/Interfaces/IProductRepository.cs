using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        public Task<bool> DeleteProdAndRelatedData(int id);
        public  Task<int> Upsert(Product product, int[] cats);

    }
}
