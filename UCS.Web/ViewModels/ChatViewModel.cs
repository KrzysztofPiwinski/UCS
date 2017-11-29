using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class ChatViewModel
    {
        public string Content { get; set; }
        public string Author { get; set; }
        public string AddedAt { get; set; }
        public bool IsFromMe { get; set; }
        public string SavedFileName { get; set; }
        public string RealFileName { get; set; }

        public static ChatViewModel FromDb(Chat chatDb, string userId = null, int studentId = 0)
        {
            ChatViewModel model = new ChatViewModel()
            {
                Content = chatDb.Content,
                AddedAt = chatDb.AddedAt.ToString("dd.MM.yyyy HH:mm:ss"),
                Author = chatDb.SenderStudent != null ? chatDb.SenderStudent.FirstName + " " + chatDb.SenderStudent.LastName : chatDb.SenderUser.FirstName + " " + chatDb.SenderUser.LastName,
                RealFileName = chatDb.File != null ? chatDb.File.RealFileName : string.Empty,
                SavedFileName = chatDb.File != null ? chatDb.File.SavedFileName : string.Empty
            };

            if (!string.IsNullOrEmpty(userId))
            {
                model.IsFromMe = chatDb.SenderUserId == userId;
            }
            else if (studentId != 0)
            {
                model.IsFromMe = chatDb.SenderStudentId == studentId;
            }

            return model;
        }
    }
}