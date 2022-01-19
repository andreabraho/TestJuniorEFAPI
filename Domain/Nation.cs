using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Nation:EntityBase
    {
        public string Name { get; set; }
        public ICollection<InfoRequest> InfoRequests { get; set; }
    }
}
