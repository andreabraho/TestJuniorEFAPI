using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class InfoRequest:EntityBase
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Cap { get; set; }
        public string RequestText { get; set; }
        public DateTime InsertDate { get; set; }
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
