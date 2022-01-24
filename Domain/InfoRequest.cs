using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// rappresents info request sended by users for information about a product
    /// </summary>
    public class InfoRequest:EntityBase
    {
        /// <summary>
        /// info request sender name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// info request sender last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// info request sender email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// info request sender city
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// info request sender postal code
        /// </summary>
        public string Cap { get; set; }
        public string RequestText { get; set; }
        /// <summary>
        /// inserting date of info request
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// info request sender phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int NationId { get; set; }
        public Nation Nation { get; set; }
        public ICollection<InfoRequestReply> InfoRequestReplys { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
