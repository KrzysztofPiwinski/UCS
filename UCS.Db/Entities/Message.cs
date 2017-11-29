using System;
using System.Collections.Generic;

namespace UCS.Db.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public DateTime SendAt { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<string> Emails { get; set; }
    }
}
