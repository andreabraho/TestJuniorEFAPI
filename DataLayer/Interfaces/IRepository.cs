using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetById(int id);
        Task<int> Insert(T entity);
        Task<int> Update(T entity);
        Task<int> DeleteAsync(int id);
       
    }
}
