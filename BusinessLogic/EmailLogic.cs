using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace EthioProductShoppingCenter.BusinessLogic
{
    public class EmailLogic
    {
        public async Task EmailReceiverAsync(string from, string subject, string body)
        {
            MailMessage mail = new MailMessage(from, "girma.m.halie19@gmail.com");
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential()
            {
                UserName = "girma.m.halie19@gmail.com",
                Password = "Getachew19@"
            };

            client.EnableSsl = true;
            await client.SendMailAsync(mail);

        }
    }
}