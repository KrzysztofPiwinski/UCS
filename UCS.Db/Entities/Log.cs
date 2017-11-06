using System;

namespace UCS.Db.Entities
{
    class Log
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public string Description { get; set; }
    }
}
