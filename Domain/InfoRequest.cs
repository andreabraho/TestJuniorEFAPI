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
        public string _name;
        public string Name { get { return _name; }
            set { ValidateName(value);_name = value; } }
        /// <summary>
        /// info request sender last name
        /// </summary>
        private string _lastName;
        public string LastName { get { return _lastName; }
            set { ValidateLastName(value);_lastName = value; } }
        /// <summary>
        /// info request sender email
        /// </summary>
        private string _email;
        public string Email { get { return _email; } 
            set { VaidateEmail(value);_email = value; } }
        /// <summary>
        /// info request sender city
        /// </summary>
        public string _city;
        public string City { get { return _city; } 
            set { ValidateCity(value);_city = value; } }
        /// <summary>
        /// info request sender postal code
        /// </summary>
        public string _cap;
        public string Cap { get { return _cap; }
            set { ValidateCap(value);_cap = value; } }
        public string _requestText;
        public string RequestText { get { return _requestText; }
            set { ValidateRequestText(value);_requestText = value; } }
        /// <summary>
        /// inserting date of info request
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// info request sender phone number
        /// </summary>
        private string _phoneNumber;
        public string PhoneNumber { get { return _phoneNumber; }
            set { ValidatePhone(value);_phoneNumber = value; } }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int NationId { get; set; }
        public Nation Nation { get; set; }
        public IEnumerable<InfoRequestReply> InfoRequestReplys { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }

        private void ValidateName(string name)
        {
            if(string.IsNullOrWhiteSpace(name) || name.Length>255)
                throw new ArgumentException(nameof(name));
        }
        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > 255)
                throw new ArgumentException(nameof(lastName));
        }
        private void VaidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length > 255)
                throw new ArgumentException(nameof(email));
            
        }
        private void ValidateCity(string city)
        {
            if (city.Length == 0 || city.Length > 189)
                throw new ArgumentException(nameof(city));
            
        }
        private void ValidatePhone(string phone)
        {
            if (phone.Length == 0 || phone.Length > 15)
                throw new ArgumentException(nameof(phone));

        }
        private void ValidateCap(string cap)
        {
            if (cap.Length == 0 || cap.Length > 15)
                throw new ArgumentException(nameof(cap));

        }
        private void ValidateRequestText(string requestText)
        {
            if (string.IsNullOrWhiteSpace(requestText) || requestText.Length > 255)
                throw new ArgumentException(nameof(requestText));
        }
    }
}
