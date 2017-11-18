using System;
using System.Collections.Generic;

namespace UCS.Db.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime LastActivity { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual List<StudentCategory> Categories { get; set; }
    }
}
