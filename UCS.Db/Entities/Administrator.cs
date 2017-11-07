using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UCS.Db.Entities
{
    public class Administrator : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public virtual List<Permission> Permissions { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Administrator> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
