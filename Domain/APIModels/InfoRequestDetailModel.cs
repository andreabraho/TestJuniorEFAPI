using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.APIModels
{
    public class InfoRequestDetailModel
    {
        public int Id { get; set; }
        public ProductIRDetail productIRDetail { get; set; }
        public string Name { get; set; }    
        public string LastName { get; set; }    
        public string Email { get; set; }
        public string Location { get; set; }
        public List<IRModelReply> IRModelReplies { get; set; }
    }
    public class ProductIRDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }

    }
    public class IRModelReply
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string ReplyText { get; set; }
    }
}
