using System.Collections.Generic;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class UsersViewModel : LayoutViewModel
    {
        public List<UserViewModel> Users { get; set; }

        public static UsersViewModel FromDb(List<User> usersDb)
        {
            UsersViewModel usersModel = new UsersViewModel()
            {
                Users = new List<UserViewModel>()
            };

            foreach (User userDb in usersDb)
            {
                UserViewModel userModel = UserViewModel.FromDb(userDb);

                usersModel.Users.Add(userModel);
            }

            return usersModel;
        }
    }
}