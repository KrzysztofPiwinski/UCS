using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using UCS.Db.Entities;

namespace UCS.Db
{
    public class UCSContext : IdentityDbContext<Administrator>
    {
        public DbSet<Student> Students { get; set; }

        public static UCSContext Create()
        {
            return new UCSContext();
        }
    }
}
