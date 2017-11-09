using System;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedAt { get; set; }

        public static UserViewModel FromDb(User userDb)
        {
            UserViewModel model = new UserViewModel()
            {
                Id = userDb.Id,
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,
                Email = userDb.Email,
                AddedAt = userDb.AddedAt,
                IsActive = userDb.IsActive
            };

            return model;
        }
    }
}