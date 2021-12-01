using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet_web_api.Models;

namespace aspnet_web_api.Utility
{
    public class GoogleSTMP
    {
        public bool SendEmail(Email model)
        {
            using (MailMessage mailMessage = new MailMessage("blueshop.tech@gmail.com", model.To))
            {
                try
                {
                    mailMessage.Subject = model.Subject;
                    mailMessage.Body = model.Body;
                    mailMessage.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("blueshop.tech@gmail.com", "123Welkom!");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mailMessage);
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
     
            }
        }
    }
}
