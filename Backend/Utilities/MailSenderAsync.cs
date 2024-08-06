using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class MailSenderAsync
    {
        public Task SendMailAsync(string mailTo, string subject,string body)
        {
            var mailMessage = new MailMessage();    
            mailMessage.From = new MailAddress("gearedfinance@util.com");
            mailMessage.To.Add(new MailAddress(mailTo));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = "mail.etatvasoft.com";
                smtpClient.Port =587;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("aksh.patel@etatvasoft.com", "s4l5eejl}j@V");
                smtpClient.Send(mailMessage);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            }
                return Task.CompletedTask; 
        }
    }
}
