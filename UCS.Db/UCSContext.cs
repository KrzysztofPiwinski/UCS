using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using UCS.Db.Entities;

namespace UCS.Db
{
    public class UCSContext : IdentityDbContext<User>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageFile> MessageFiles { get; set; }
        public DbSet<MessageLog> MessageLogs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StudentCategory> StudentCategories { get; set; }

        public static UCSContext Create()
        {
            return new UCSContext();
        }
    }
}
