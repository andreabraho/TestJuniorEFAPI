﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.APIModels
{
    /// <summary>
    /// data needed in product detail page
    /// model used to create the object that will be returned in Produc API Product/Detail/{id}
    /// </summary>
    public class ProductDetailModel
    {
        /// <summary>
        /// product id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// brand name associated to the prodyct 
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// categories associated to the product 
        /// </summary>
        public List<Category> productsCategory  { get; set; }
        /// <summary>
        /// number of info requests recived from the profuct from guest users
        /// </summary>
        public int countGuestInfoRequests { get; set; }
        /// <summary>
        /// number of info requests recived from the profuct from registered users
        /// </summary>
        public int countUserInfoRequests { get; set; }
        /// <summary>
        /// list of info requests recived from the product
        /// </summary>
        public List<InfoRequestProductModel> infoRequestProducts { get; set; } = new List<InfoRequestProductModel>();

    }
    /// <summary>
    /// info request data neccessary for product detail page
    /// </summary>
    public class InfoRequestProductModel
    {
        /// <summary>
        /// info request id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// info request sender name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// info request sender last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// number of reply to the info request
        /// </summary>
        public int ReplyNumber { get; set; }
        /// <summary>
        /// date of last reply of the info request
        /// </summary>
        public DateTime DateLastReply   { get; set; }
    }
}
