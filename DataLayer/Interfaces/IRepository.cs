using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DataLayer.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
        public T Execute<T>(IQueryable<T> query);
        public List<T> ExecuteList<T>(IQueryable<T> query);
    }
}
