using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class InfoRequestReply:EntityBase
    {
        public string ReplyText { get; set; }
        public DateTime InsertDate { get; set; }
        public int InfoRequestId { get; set; }
        public InfoRequest InfoRequest { get; set; }
        public int? AccountId { get; set; }
        public Account Account { get; set; }

    }
}
