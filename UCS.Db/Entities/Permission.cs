namespace UCS.Db.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public PermissionEnum Permiss { get; set; }
    }
}
