﻿using System;

namespace UCS.Db.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public string Description { get; set; }
    }
}
