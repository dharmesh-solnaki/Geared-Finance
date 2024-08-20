using System.Net;
using System.Net.Mail;

namespace Utilities
{
    public static class MailSenderAsync
    {
        public static Task SendMailAsync(string mailTo, string subject, string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("gearedfinance@service.com");
            mailMessage.To.Add(new MailAddress(mailTo));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = "mail.etatvasoft.com";
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("dharmesh.solanki@tatvasoft.com", "github@025");
                smtpClient.Send(mailMessage);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            }
            return Task.CompletedTask;
        }
    }
}
