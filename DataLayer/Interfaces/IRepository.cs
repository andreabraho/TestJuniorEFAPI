﻿using Domain;
using System;
using System.Collections.Generic;
using System.Text;
namespace DataLayer.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
