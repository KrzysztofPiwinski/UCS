namespace UCS.Db.Entities
{
    class MessageFile
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
    }
}
