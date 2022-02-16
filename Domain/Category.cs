using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Category:EntityBase
    {
        public string _name;
        public string Name { get { return _name; }
            set {ValidateName(value);_name = value; } }   
        public IEnumerable<ProductCategory> Product_Categories { get; set; }
        
        private void ValidateName(string name)
        {
            if(string.IsNullOrEmpty(name) || name.Length>255)
                throw new ArgumentException(nameof(name));
        }
    }
}
