using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AddedAt { get; set; }
        public string AddedBy { get; set; }

        public static MessageViewModel FromDb(Message messageDb)
        {
            MessageViewModel model = new MessageViewModel()
            {
                Id = messageDb.Id,
                Title = messageDb.Title,
                Content = messageDb.Content,
                AddedAt = messageDb.AddedAt.ToString("dd.MM.yyyy HH.mm"),
                AddedBy = messageDb.User.UserName
            };

            return model;
        }
    }
}