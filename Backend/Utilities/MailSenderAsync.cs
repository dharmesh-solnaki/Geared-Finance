using System.Net;
using System.Net.Mail;

namespace Utilities
{
    public static class MailSenderAsync
    {
        public static Task SendMailAsync(string mailTo, string subject, string body, EmailSettings emailSettings)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(Constants.SERVICE_EMAIL)
            };
            mailMessage.To.Add(new MailAddress(mailTo));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            using (SmtpClient smtpClient = new())
            {
                smtpClient.Host = emailSettings.MailHost;
                smtpClient.Port = emailSettings.MailPort;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(emailSettings.MailId, emailSettings.MailPassword);
                smtpClient.Send(mailMessage);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            }
            return Task.CompletedTask;
        }
    }

    public class EmailSettings
    {
        public string MailHost { get; set; } = null!;
        public int MailPort { get; set; }
        public string MailId { get; set; } = null!;
        public string MailPassword { get; set; } = null!;
    }
}
