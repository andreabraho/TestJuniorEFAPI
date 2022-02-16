using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// rappresent one reply to a info request it can be from a brand or a user
    /// </summary>
    public class InfoRequestReply:EntityBase
    {
        private string _replyText;
        public string ReplyText { get { return _replyText; }
            set { ValidateReply(value);_replyText = value; } }
        public DateTime InsertDate { get; set; }
        public int InfoRequestId { get; set; }
        public InfoRequest InfoRequest { get; set; }
        public int? AccountId { get; set; }
        public Account Account { get; set; }

        private void ValidateReply(string reply)
        {
            if (string.IsNullOrEmpty(reply) || reply.Length>255)
                throw new ArgumentException(nameof(reply));
        }

    }
}
