using System;
using System.Collections.Generic;
using System.Text;

namespace ServicaLayer.InfoRequestService.Model
{
    /// <summary>
    /// all data needed in info request detail page
    /// model used to create the object that will be returned in Produc API InfoRequest/Detail/{id}
    /// </summary>
    public class InfoRequestDetailDTO
    {
        /// <summary>
        /// info request id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// all data of the product subject of the info request
        /// </summary>
        public ProductIRDetailDTO productIRDetail { get; set; }
        /// <summary>
        /// info request sender name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// info request sender lastName
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// info request sender email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// string format containing “City (CAP), nation”
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// list of all replies of the info request
        /// </summary>
        public IEnumerable<IRModelReplyDTO> IRModelReplies { get; set; }
    }
    /// <summary>
    /// product data needed for the info request detail page
    /// </summary>
    public class ProductIRDetailDTO
    {
        /// <summary>
        /// product id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product name
        /// </summary>
        public string Name { get; set; }
        public string BrandName { get; set; }

    }
    /// <summary>
    /// info request reply data needed for info request detail page
    /// </summary>
    public class IRModelReplyDTO
    {
        /// <summary>
        /// info request reply id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// user sending the reply
        /// </summary>
        public string User { get; set; }
        public string ReplyText { get; set; }
    }
}
