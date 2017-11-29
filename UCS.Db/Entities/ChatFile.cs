namespace UCS.Db.Entities
{
    public class ChatFile
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string SavedFileName { get; set; }
        public string RealFileName { get; set; }
    }
}
