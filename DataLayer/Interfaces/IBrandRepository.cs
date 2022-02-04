using DataLayer.Repository;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        public Task<bool> InsertWithProducts(Account account,Brand brand, ProdWithCat[] products);
        public Task<bool> DeleteAll(int id);


    }
}
