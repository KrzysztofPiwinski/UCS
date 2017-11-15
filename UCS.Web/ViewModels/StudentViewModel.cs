using System;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime LastActivity { get; set; }
        public DateTime? DeletedAt { get; set; }

        public static StudentViewModel FromDb(Student studentDb)
        {
            StudentViewModel model = new StudentViewModel()
            {
                Id = studentDb.Id,
                FirstName = studentDb.FirstName,
                LastName = studentDb.LastName,
                UserName = studentDb.UserName,
                IsActive = studentDb.IsActive,
                AddedAt = studentDb.AddedAt,
                LastActivity = studentDb.LastActivity,
                DeletedAt = studentDb.DeletedAt
            };

            return model;
        }
    }
}