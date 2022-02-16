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
        private string _email;
        public string Email {   get { return _email; }
                                set {
                                    ValidateEmail(value);
                                    _email = value;
                                } 
                            }
        private string _password;
        public string Password {get { return _password; }
                                set { 
                                    ValidatePassword(value);
                                    _password= value;
                                    }
                                }
        /// <summary>
        /// 1 for brand 2 for user
        /// </summary>
        public byte AccountType { get; set; }
        public User User { get; set; }
        public Brand Brand { get; set; }
        public IEnumerable<InfoRequestReply> InfoRequestReplies { get; set; }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length>255)
                throw new ArgumentException(nameof(email),"email can't have more then 255 characters");
        }
        private void ValidatePassword(string pass)
        {
            if (string.IsNullOrWhiteSpace(pass) || pass.Length>18)
                throw new ArgumentException(nameof(pass),"password can't have more tha 18 characters");
        }
    }
}
