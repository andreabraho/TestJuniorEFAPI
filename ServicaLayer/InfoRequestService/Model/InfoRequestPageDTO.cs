using System;
using System.Collections.Generic;
using System.Text;

namespace ServicaLayer.InfoRequestService.Model
{
    public class InfoRequestPageDTO
    {
        /// <summary>
        /// page needed
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// number of info requests for each page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// total number of info requests
        /// </summary>
        public int TotalinfoRequests { get; set; }
        public int TotalPages { get; set; }

        public IEnumerable<IRForPageModel> infoRequests { get; set; }



    }
    public class IRForPageModel
    {
        public int Id { get; set; }
        public string RequestText { get; set; }
        public DateTime InsertDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }   
        public string BrandName { get; set; }

        public UserModelForIR User { get; set; }

    }
    public class UserModelForIR
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Cap { get; set; }

    }
}
