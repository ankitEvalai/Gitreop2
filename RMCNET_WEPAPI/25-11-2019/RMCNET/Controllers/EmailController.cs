using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace RMCNET.Controllers
{
    public class EmailController : ApiController
    {
        [HttpGet]
        public string SendEmail(string toAddress, string subject, string body)
        {
            //string toAddress = "ramkumar@evalai.com";
            //string subject = "Mail from C#";
            //string body = "Sample mail from c#";
            string result = "Message Sent Successfully..!!";
            string senderID = "thevalaiinfotech@gmail.com";// use sender’s email id here..
            const string senderPassword = "Thevalai!nfotech"; // sender password here…
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // smtp server address here…
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailAddress from = new MailAddress("thevalaiinfotech@gmail.com", "RMCNET");
                MailMessage message = new MailMessage(from.ToString(), toAddress, subject, body);
                //MailAddress bcc = new MailAddress("vasu@evalai.com");
                //message.Bcc.Add(bcc);
                MailAddress bcc = new MailAddress("ravikiran@evalai.com");
                message.Bcc.Add(bcc);
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!!";
            }
            return result;
        }

    }
}
