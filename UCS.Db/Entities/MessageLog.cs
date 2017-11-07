using System;

namespace UCS.Db.Entities
{
    public class MessageLog
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public int MessageId { get; set; }
        public virtual Message Message { get; set; }
        public int? AdministratorId { get; set; }
        public int? StudentId { get; set; }
        public bool IsRead { get; set; }
    }
}
