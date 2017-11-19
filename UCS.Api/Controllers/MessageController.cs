using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using UCS.Api.Models;
using UCS.Db;
using UCS.Db.Entities;

namespace UCS.Api.Controllers
{
    public class MessageController : ApiController
    {
        private UCSContext context = null;

        public MessageController() : base()
        {
            context = new UCSContext();
        }

        public HttpResponseMessage Post(MessageDTO message)
        {
            HttpResponseMessage response = null;

            if (message == null || message.CategoryId < 1 || !context.Categories.Any(c => c.Id == message.CategoryId)
                || string.IsNullOrEmpty(message.Content) || string.IsNullOrEmpty(message.Title) || string.IsNullOrEmpty(message.UserId))
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }

            User userDb = context.Users.SingleOrDefault(u => u.Id == message.UserId);
            if (userDb == null)
            {
                response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return response;
            }

            List<Student> studentsDb = context.Students.Where(s => s.Categories.Any(c => c.CategoryId == message.CategoryId)).ToList();

            Message messageDb = new Message()
            {
                Title = message.Title,
                Content = message.Content,
                Emails = studentsDb.Select(s => s.UserName).ToList(),
                AddedAt = DateTime.Now,
                UserId = message.UserId,
                SendAt = DateTime.Now
            };

            userDb.Messages.Add(messageDb);
            context.SaveChanges();

            foreach (string email in studentsDb.Select(s => s.UserName))
            {
                //MailHelpers.Send(email, messageDb.Title, messageDb.Content);
            }

            response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(studentsDb.Count().ToString(), Encoding.UTF8);

            return response;
        }
    }
}