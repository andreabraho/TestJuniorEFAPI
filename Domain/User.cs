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
        public string _name;
        public string Name { get { return _name; }
            set { ValidateString(value);_name = value; } }
        public string _lastName;
        public string LastName { get { return _lastName; }
            set { ValidateString(value);_lastName = value; } }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public IEnumerable<InfoRequest> InfoRequests { get; set; }
        private void ValidateString(string str)
        {
            if(str.Length==0 || str.Length>255)
                throw new ArgumentException(nameof(str));
        }


    }
}
