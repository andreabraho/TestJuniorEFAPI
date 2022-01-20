using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        public BrandDetail GetBrandDetail(int id);
        public List<Brand> GetPageBrands(int page, int pageSize);
        public int GetCount();
    }
}
