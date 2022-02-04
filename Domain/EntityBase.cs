using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class EntityBase
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
