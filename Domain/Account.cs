using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// rappresent database table account 
    /// </summary>
    public class Account:EntityBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 1 for brand 2 for user
        /// </summary>
        public byte AccountType { get; set; }
        public User User { get; set; }
        public Brand Brand { get; set; }
        public IEnumerable<InfoRequestReply> InfoRequestReplies { get; set; }
    }
}
