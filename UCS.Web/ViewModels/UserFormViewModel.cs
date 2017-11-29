using System.Collections.Generic;
using System.Linq;
using UCS.Db.Entities;
using UCS.Web.Models;

namespace UCS.Web.ViewModels
{
    public class UserFormViewModel : LayoutViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public List<PermissionEnum> AllPermissions { get; set; }
        public List<PermissionEnum> UserPermission { get; set; }
        public ActionTypeEnum ActionType { get; set; }

        public static UserFormViewModel FromDb(User userDb)
        {
            UserFormViewModel model = new UserFormViewModel()
            {
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,
                Email = userDb.Email,
                IsActive = userDb.IsActive,
                UserPermission = userDb.Permissions.Select(p => p.Permiss).ToList()
            };

            return model;
        }
    }
}