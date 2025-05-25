using System.Net.Mail;
using System.Net;

namespace WebApplication2.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("giangquangdao0211@gmail.com", "ravbmziojixqaawn")
            };

            return client.SendMailAsync(
                new MailMessage(from: "giangquangdao0211@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
