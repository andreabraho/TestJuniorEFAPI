using DataLayer.Repository;
using Domain;
using Domain.ModelsForApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        public Task<int> InsertWithProducts(Account account,Brand brand, ProdWithCat[] products);
        public Task<bool> DeleteBrandAndRelatedData(int id);
        public Task<bool> ValidateEmailExistence(string email);

    }
}
