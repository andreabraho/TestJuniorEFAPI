using DataLayer.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<int> Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            entities.Add(entity);
            
            return await _ctx.SaveChangesAsync();
        }
        public async Task<int> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            return await _ctx.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(int id)
        {

            T entity = entities.SingleOrDefault(s => s.Id == id);
            if(entity != null)
            entity.IsDeleted = true;
            return await _ctx.SaveChangesAsync();
        }
        


    }

}
