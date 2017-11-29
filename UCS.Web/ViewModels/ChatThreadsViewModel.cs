using System;
using System.Collections.Generic;
using System.Linq;
using UCS.Db.Entities;
using UCS.Web.Models;

namespace UCS.Web.ViewModels
{
    public class ChatThreadsViewModel : LayoutViewModel
    {
        public List<ChatThreadViewModel> Threads { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public ChatRecipientEnum ChatRecipient { get; set; }

        public static ChatThreadsViewModel FromDb(List<Chat> chatsDb, string userId = null, int studentId = 0)
        {
            ChatThreadsViewModel model = new ChatThreadsViewModel();
            model.Threads = new List<ChatThreadViewModel>();

            foreach (Chat chatDb in chatsDb)
            {
                model.Threads.Add(ChatThreadViewModel.FromDb(chatDb, userId, studentId));
            }

            return model;
        }

        public static ChatThreadsViewModel FromDb(List<IGrouping<int, Chat>> threads, string userId = null, int studentId = 0)
        {
            ChatThreadsViewModel model = new ChatThreadsViewModel();
            model.Threads = new List<ChatThreadViewModel>();

            foreach (IGrouping<int, Chat> thread in threads)
            {
                model.Threads.Add(ChatThreadViewModel.FromDb(thread, userId, studentId));
            }

            model.Threads = model.Threads.OrderBy(t => t.IsRead).ThenByDescending(t => t.AddedAt).ToList();
            return model;
        }
    }
}