using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UCS.Db;
using UCS.Db.Entities;

namespace UCS.Web.Models.Repositories
{
    public class UserRepository
    {
        private UCSContext _context = new UCSContext();

        public List<User> GetAll()
        {
            return _context.Users.OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName).ToList();
        }

        public User GetById(string id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }

        public User GetByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public bool Add(User userDb, string password)
        {
            UserStore<User> userStore = new UserStore<User>(_context);
            UserManager<User> userManager = new UserManager<User>(userStore);

            IdentityResult result = userManager.Create(userDb, password);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Edit(User userDb)
        {
            userDb.LastModifiedAt = DateTime.Now;
            _context.SaveChanges();
        }

        public void RemovePermissionById(User userDb)
        {
            _context.Permissions.RemoveRange(userDb.Permissions);
            _context.SaveChanges();
        }

        public string ResetPassword(User userDb)
        {
            string password = Guid.NewGuid().ToString("N").Substring(0, 8);
            UserStore<User> userStore = new UserStore<User>(_context);
            UserManager<User> userManager = new UserManager<User>(userStore);
            userManager.RemovePassword(userDb.Id);
            userManager.AddPassword(userDb.Id, password);

            return password;
        }
    }
}