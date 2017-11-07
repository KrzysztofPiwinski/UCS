namespace UCS.Db.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public int AdministratorId { get; set; }
        public PermissionEnum Permiss { get; set; }
    }
}
