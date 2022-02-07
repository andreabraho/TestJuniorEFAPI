// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;

namespace DataLayer.QueryObjects
{
    public static class GenericPaging
    {
        /// <summary>
        /// get single page for paging items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">page start 1</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">page size not valid</exception>
        /// <exception cref="ArgumentOutOfRangeException">page not valid</exception>
        public static IQueryable<T> Page<T>(
            this IQueryable<T> query,
            int page, int pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException
                    (nameof(pageSize), "pageSize cannot be lower or equal than zero.");
            if (page <=0)
                throw new ArgumentOutOfRangeException
                    (nameof(page), "page cannot be lower or equal than zero.");

            
            query = query
                .Skip((page-1) * pageSize); 

            return query.Take(pageSize); 
        }

    }
}