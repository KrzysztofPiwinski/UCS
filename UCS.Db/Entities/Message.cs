using System;
using System.Collections.Generic;

namespace UCS.Db.Entities
{
    class Message
    {
        public int Id { get; set; }
        public int? RootMessageLog { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public DateTime SendAt { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsEmail { get; set; }
        public int? AdministratorId { get; set; }
        public virtual Administrator Administrator { get; set; }
        public int? StudentId { get; set; }
        public virtual Student Student { get; set; }
        public virtual List<MessageLog> Logs { get; set; }
        public virtual List<MessageFile> Files { get; set; }
    }
}
