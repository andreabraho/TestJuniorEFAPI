using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<InfoRequest> InfoRequests { get; set; }
    }
}
