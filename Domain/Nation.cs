using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Nation:EntityBase
    {
        public string Name { get; set; }
        public IEnumerable<InfoRequest> InfoRequests { get; set; }
    }
}
