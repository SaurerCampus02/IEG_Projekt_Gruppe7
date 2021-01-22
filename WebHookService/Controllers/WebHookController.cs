using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using WebHookService.Data;
using WebHookService.Models;

namespace WebHookService.Controllers
{
    [ApiController]
    [Route("api/[controller]/{action}")]
    public class WebHookController : ControllerBase
    {
        private readonly EmailStorage emailStorage;

        public WebHookController(EmailStorage emailStorage)
        {
            this.emailStorage = emailStorage;
        }

        [HttpPost]
        public ActionResult<IEnumerable<Emails>> Register([FromBody] Emails emails)
        {
            Console.WriteLine(emails);
            emailStorage.Emails.Add(emails);
            return emailStorage.Emails;
        }

        [HttpPost]
        public OkResult SendMail()
        {
            var smtp = new SmtpClient
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Host = "smtp.live.com",
                Port = 25,
                // here should be credentials from email
                Credentials = new NetworkCredential("", ""),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            foreach (var message in emailStorage.Emails.Select(email => new MailMessage(email.Mail, email.Mail)))
            {
                message.Subject = "WebHook got called";
                message.Body = "WebHook got called";
                smtp.Send(message);
            }

            return Ok();
        }
    }
}