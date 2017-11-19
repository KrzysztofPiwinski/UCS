using System.Collections.Generic;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class MessagesViewModel : LayoutViewModel
    {
        public List<MessageViewModel> Messages { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }

        public static MessagesViewModel FromDb(List<Message> messagesDb)
        {
            MessagesViewModel model = new MessagesViewModel();
            model.Messages = new List<MessageViewModel>();

            foreach (Message messageDb in messagesDb)
            {
                model.Messages.Add(MessageViewModel.FromDb(messageDb));
            }

            return model;
        }
    }
}