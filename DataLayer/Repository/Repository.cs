using DataLayer.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repository
{

    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly MyContext _ctx;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public Repository(MyContext context)
        {
            this._ctx = context;
            entities = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return entities.AsQueryable();
        }
        public IQueryable<T> GetById(int id)
        {
            return entities.Where(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            entities.Add(entity);
            _ctx.SaveChanges();
        }
        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _ctx.SaveChanges();
        }
        public void Delete(int id)
        {

            T entity = entities.SingleOrDefault(s => s.Id == id);
            entities.Remove(entity);
            _ctx.SaveChanges();
        }
        public T Execute<T>(IQueryable<T> query)
        {
            return query.OfType<T>().FirstOrDefault();
        }

        public List<T> ExecuteList<T>(IQueryable<T> query)
        {
            return query.OfType<T>().ToList();
        }


    }

}
