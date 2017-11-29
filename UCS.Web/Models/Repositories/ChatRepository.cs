using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCS.Db;
using UCS.Db.Entities;

namespace UCS.Web.Models.Repositories
{
    public class ChatRepository
    {
        private UCSContext _context = null;

        public ChatRepository() : base()
        {
            _context = new UCSContext();
        }

        public List<IGrouping<int, Chat>> GetAllStudentsForAdmin(string id)
        {
            List<IGrouping<int, Chat>> a = _context.Chats.GroupBy(c => c.ParentChatId).ToList();

            List<IGrouping<int, Chat>> returningThreads = new List<IGrouping<int, Chat>>();

            foreach (IGrouping<int, Chat> chats in a)
            {
                if (chats.Any(c => (c.ReceiverUserId == id || c.SenderUserId == id) && (c.SenderStudentId != null || c.ReceiverStudentId != null)))
                {
                    returningThreads.Add(chats);
                }
            }

            return returningThreads;
        }

        public List<IGrouping<int, Chat>> GetAllAdminsForAdmin(string id)
        {
            List<IGrouping<int, Chat>> a = _context.Chats.GroupBy(c => c.ParentChatId).ToList();

            List<IGrouping<int, Chat>> returningThreads = new List<IGrouping<int, Chat>>();

            foreach (IGrouping<int, Chat> chats in a)
            {
                if (chats.Any(c => (c.ReceiverUserId == id || c.SenderUserId == id) && c.SenderStudentId == null && c.ReceiverStudentId == null))
                {
                    returningThreads.Add(chats);
                }
            }

            return returningThreads;
        }

        public List<IGrouping<int, Chat>> GetAllAdminsForStudent(int id)
        {
            List<IGrouping<int, Chat>> threads = _context.Chats.GroupBy(c => c.ParentChatId).ToList();

            List<IGrouping<int, Chat>> returningThreads = new List<IGrouping<int, Chat>>();

            foreach (IGrouping<int, Chat> thread in threads)
            {
                if (thread.Any(c => (c.ReceiverStudentId == id || c.SenderStudentId == id) && (!string.IsNullOrEmpty(c.SenderUserId) || !string.IsNullOrEmpty(c.ReceiverUserId))))
                {
                    returningThreads.Add(thread);
                }
            }

            return returningThreads;
        }

        public List<IGrouping<int, Chat>> GetAllStudentsForStudent(int id)
        {
            List<IGrouping<int, Chat>> threads = _context.Chats.GroupBy(c => c.ParentChatId).ToList();

            List<IGrouping<int, Chat>> returningThreads = new List<IGrouping<int, Chat>>();

            foreach (IGrouping<int, Chat> thread in threads)
            {
                if (thread.Any(c => (c.ReceiverStudentId == id || c.SenderStudentId == id) && string.IsNullOrEmpty(c.SenderUserId) && string.IsNullOrEmpty(c.ReceiverUserId)))
                {
                    returningThreads.Add(thread);
                }
            }

            return returningThreads;
        }

        public bool CheckIfIsMy(int id, string userId = null, int studentId = 0)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return _context.Chats.SingleOrDefault(c => c.Id == id && (c.ReceiverUserId == userId || c.SenderUserId == userId)) != null;
            }
            else if (studentId != 0)
            {
                return _context.Chats.SingleOrDefault(c => c.Id == id && (c.SenderStudentId == studentId || c.ReceiverStudentId == studentId)) != null;
            }

            return false;
        }

        public List<Chat> GetAllThreadById(int id, string userId = null, int studentId = 0)
        {
            Chat chatDb = _context.Chats.SingleOrDefault(c => c.Id == id);
            List<Chat> chatsDb = _context.Chats.Where(c => c.ParentChatId == chatDb.ParentChatId).OrderBy(c => c.AddedAt).ToList();

            if (!string.IsNullOrEmpty(userId))
            {
                chatsDb.Where(c => c.ReceiverUserId == userId).All(c => c.IsRead = true);
                _context.SaveChanges();
            }
            else if (studentId != 0)
            {
                chatsDb.Where(c => c.ReceiverStudentId == studentId).All(c => c.IsRead = true);
                _context.SaveChanges();
            }
            return chatsDb;
        }

        public void Add(Chat chatDb)
        {
            chatDb.AddedAt = DateTime.Now;
            _context.Chats.Add(chatDb);
            _context.SaveChanges();
        }

        public Chat GetById(int parentId)
        {
            return _context.Chats.SingleOrDefault(c => c.Id == parentId);
        }

        public void AddParentId(Chat chatDb)
        {
            _context.SaveChanges();
        }
    }
}