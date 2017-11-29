using System;
using System.Linq;
using UCS.Db.Entities;

namespace UCS.Web.ViewModels
{
    public class ChatThreadViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AddedAt { get; set; }
        public bool IsRead { get; set; }

        public static ChatThreadViewModel FromDb(Chat chatDb, string userId = null, int studentId = 0)
        {
            ChatThreadViewModel model = new ChatThreadViewModel()
            {
                Id = chatDb.Id,
                Title = chatDb.Title,
                AddedAt = chatDb.AddedAt.ToString("dd.MM.yyyy HH:mm:ss")
            };

            if (!string.IsNullOrEmpty(userId))
            {
                if (chatDb.SenderUserId == userId)
                {
                    model.Author = chatDb.ReceiverUser != null ? chatDb.ReceiverUser.FirstName + " " + chatDb.ReceiverUser.LastName : chatDb.ReceiverStudent.FirstName + " " + chatDb.ReceiverStudent.LastName;
                }
                else if (chatDb.ReceiverUserId == userId)
                {
                    model.Author = chatDb.SenderUser != null ? chatDb.SenderUser.FirstName + " " + chatDb.SenderUser.LastName : chatDb.SenderStudent.FirstName + " " + chatDb.SenderStudent.LastName;
                }
            }

            if (studentId != 0)
            {
                if (chatDb.SenderStudentId == studentId)
                {
                    model.Author = chatDb.ReceiverStudent != null ? chatDb.ReceiverStudent.FirstName + " " + chatDb.ReceiverStudent.LastName : chatDb.ReceiverUser.FirstName + " " + chatDb.ReceiverUser.LastName;
                }
                else if (chatDb.ReceiverStudentId == studentId)
                {
                    model.Author = chatDb.SenderStudent != null ? chatDb.SenderStudent.FirstName + " " + chatDb.SenderStudent.LastName : chatDb.SenderUser.FirstName + " " + chatDb.SenderUser.LastName;
                }
            }

            return model;
        }

        public static ChatThreadViewModel FromDb(IGrouping<int, Chat> thread, string userId, int studentId)
        {
            ChatThreadViewModel model = new ChatThreadViewModel()
            {
                Id = thread.Last().Id,
                Title = thread.Last().Title,
                AddedAt = thread.Last().AddedAt.ToString("dd.MM.yyyy HH:mm:ss")
            };

            if (!string.IsNullOrEmpty(userId))
            {
                model.IsRead = thread.Any(t => t.ReceiverUserId == userId) ? thread.OrderBy(t => t.AddedAt).Last(t => t.ReceiverUserId == userId).IsRead : true;

                if (thread.Last().SenderUserId == userId)
                {
                    model.Author = thread.Last().ReceiverUser != null ? thread.Last().ReceiverUser.FirstName + " " + thread.Last().ReceiverUser.LastName : thread.Last().ReceiverStudent.FirstName + " " + thread.Last().ReceiverStudent.LastName;
                }
                else if (thread.Last().ReceiverUserId == userId)
                {
                    model.Author = thread.Last().SenderUser != null ? thread.Last().SenderUser.FirstName + " " + thread.Last().SenderUser.LastName : thread.Last().SenderStudent.FirstName + " " + thread.Last().SenderStudent.LastName;
                }
            }

            if (studentId != 0)
            {
                model.IsRead = thread.Any(t => t.ReceiverStudentId == studentId) ? thread.OrderBy(t => t.AddedAt).Last(t => t.ReceiverStudentId == studentId).IsRead : true;

                if (thread.Last().SenderStudentId == studentId)
                {
                    model.Author = thread.Last().ReceiverStudent != null ? thread.Last().ReceiverStudent.FirstName + " " + thread.Last().ReceiverStudent.LastName : thread.Last().ReceiverUser.FirstName + " " + thread.Last().ReceiverUser.LastName;
                }
                else if (thread.Last().ReceiverStudentId == studentId)
                {
                    model.Author = thread.Last().SenderStudent != null ? thread.Last().SenderStudent.FirstName + " " + thread.Last().SenderStudent.LastName : thread.Last().SenderUser.FirstName + " " + thread.Last().SenderUser.LastName;
                }
            }

            return model;
        }
    }
}