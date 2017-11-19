using System;
using System.Net.Mail;

namespace UCS.Api.Models
{
    public static class MailHelpers
    {

        public static void Send(string address, string title, string content)
        {
            if(!string.IsNullOrEmpty(address)||!string.IsNullOrEmpty(title)||!string.IsNullOrEmpty(content))
            {
                    MailMessage message = new MailMessage();
                    message.Subject = title;
                    message.Body = content;
                    message.To.Add(new MailAddress(address));

                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Send(message);
                    }
            
            }
        }
    }
}