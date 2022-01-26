using Domain;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {

        /// <summary>
        /// get brands for one page
        /// </summary>
        /// <param name="page">starts from 1 rappresents the page of Brands needed</param>
        /// <param name="pageSize">rapprezents the size of each page</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">in case pagesize is lower or equal than 0</exception>
        /// <exception cref="ArgumentOutOfRangeException">in case page is lower or equal than 0</exception>
        public List<Brand> GetPageBrands(int page, int pageSize);
        /// <summary>
        /// rappresents neccessary data for a brand detail page
        /// </summary>
        /// <param name="id">id of the brand needed</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">in case of id lower or equal than 0</exception>
        public BrandDetail GetBrandDetailV2(int id);
        /// <summary>
        /// rappresents neccessary data for a brand detail page
        /// </summary>
        /// <param name="id">id of the brand needed</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">in case of id lower or equal than 0</exception>
        public IQueryable<BrandDetail> GetBrandDetailV3(int id);
        public int GetCount();
    }
}
