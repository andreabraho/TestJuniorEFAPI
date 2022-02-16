using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// rappresents brand account type
    /// </summary>
    public class Brand:EntityBase
    {
        private string _brandName;
        public string BrandName { get { return _brandName; } 
                                set { ValidateBrandName(value); _brandName = value; } }
        private string _description;
        public string Description   { get { return _description; } 
            set { ValidateDescription(value); _description = value; } }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public IEnumerable<Product> Products { get; set; } 
        private void ValidateBrandName(string name)
        {
            if(string.IsNullOrWhiteSpace(name) || name.Length>255 )
                throw new ArgumentException(nameof(name));
        }
        private void ValidateDescription(string desc)
        {
            if(string.IsNullOrWhiteSpace(desc) || desc.Length>255 )
                throw new ArgumentException(nameof(desc));
        }
            
    }
}
