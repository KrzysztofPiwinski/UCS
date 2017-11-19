using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UCS.Db.Entities;

namespace UCS.Db
{
    class UCSInitializer : DropCreateDatabaseIfModelChanges<UCSContext>
    {
        private UCSContext _context = null;

        protected override void Seed(UCSContext context)
        {
            _context = context;

            List<IdentityUser> usersDB = GetAdmin();
            _context.SaveChanges();
        }

        private List<IdentityUser> GetAdmin()
        {
            UserStore<User> userStore = new UserStore<User>(_context);
            UserManager<User> userManager = new UserManager<User>(userStore);
            User admin = new User()
            {
                FirstName = "Krzysztof",
                LastName = "Piwiński",
                UserName = "krzysztof.piwinski@gmail.com",
                Email = "krzysztof.piwinski@gmail.com",
                AddedAt = DateTime.Now,
                IsActive = true,
                Permissions = new List<Permission>()
                {
                    new Permission() {Permiss=PermissionEnum.USERS }
                }
            };
            userManager.Create(admin, "qwerty123");
            return new List<IdentityUser>() { admin };
        }
    }
}
