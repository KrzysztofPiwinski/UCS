using System;
using System.Collections.Generic;

namespace UCS.Db.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public int ParentChatId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual ChatFile File { get; set; }
        public DateTime AddedAt { get; set; }
        public string SenderUserId { get; set; }
        public virtual User SenderUser { get; set; }
        public int? SenderStudentId { get; set; }
        public virtual Student SenderStudent { get; set; }
        public string ReceiverUserId { get; set; }
        public virtual User ReceiverUser { get; set; }
        public int? ReceiverStudentId { get; set; }
        public virtual Student ReceiverStudent { get; set; }
        public bool IsRead { get; set; }
    }
}
