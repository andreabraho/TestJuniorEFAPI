using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// rappresents an user account type
    /// </summary>
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public IEnumerable<InfoRequest> InfoRequests { get; set; }
    }
}
