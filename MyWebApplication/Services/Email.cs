using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace MyWebApplication.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = _config.GetValue<string>("Sender:Email");
            string fromPassword = _config.GetValue<string>("Sender:Password");

            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "PizzaZWasem.pl - " + subject;
            mailMessage.Body = "<html><body>" + htmlMessage + "</body></html>";
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(fromMail);
            mailMessage.To.Add(new MailAddress(email));

            SmtpClient client = new SmtpClient
            {
                Port = _config.GetValue<int>("ServiceEmail:Port"),
                Host = _config.GetValue<string>("ServiceEmail:Host"),
                EnableSsl = _config.GetValue<bool>("ServiceEmail:EnableSsl"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = _config.GetValue<bool>("ServiceEmail:UseDefaultCredentials"),
                Credentials = new NetworkCredential(fromMail, fromPassword)
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}