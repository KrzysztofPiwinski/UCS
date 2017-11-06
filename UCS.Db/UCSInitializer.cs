using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            UserStore<Administrator> userStore = new UserStore<Administrator>(_context);
            UserManager<Administrator> userManager = new UserManager<Administrator>(userStore);
            Administrator admin = new Administrator()
            {
                Id = "99608a63-0da7-4716-95d1-4b2c6881d65a",
                UserName = "krzysztof.piwinski@gmail.com",
                Email = "krzysztof.piwinski@gmail.com"
            };
            userManager.Create(admin, "qwerty123");
            return new List<IdentityUser>() { admin };
        }
    }
}
