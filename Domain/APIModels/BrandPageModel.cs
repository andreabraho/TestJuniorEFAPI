﻿using System.Collections.Generic;

namespace Domain.APIModels
{
    /// <summary>
    /// model used to create the object that will be returned in brand API brand/Page/{page}/{PageSize}
    /// </summary>
    public class BrandPageModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalBrand { get; set; }
        public List<BrandForPage> Brands { get; set; } = new List<BrandForPage> { };

    }
    /// <summary>
    /// data of each brand needed in brands page
    /// </summary>
    public class BrandForPage
    {
        public List<int> IdProducts { get; set; }=new List<int>();
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
