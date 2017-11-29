using System.Collections.Generic;
using System.Linq;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class ChatsViewModel : LayoutViewModel
    {
        public string Title { get; set; }
        public int ParentChatId { get; set; }
        public List<ChatViewModel> Chats { get; set; }

        public static ChatsViewModel FromDb(List<Chat> chatsDb, string userId = null, int studentId = 0)
        {
            ChatsViewModel model = new ChatsViewModel()
            {
                Title = chatsDb.First().Title,
                Chats = new List<ChatViewModel>()
            };

            foreach (Chat chatDb in chatsDb)
            {
                model.Chats.Add(ChatViewModel.FromDb(chatDb, userId, studentId));
            }

            return model;
        }
    }
}