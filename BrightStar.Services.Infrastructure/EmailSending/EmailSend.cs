using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace BrightStar.Services.Infrastructure.EmailSending
{
    public class EmailSend : IEmailSend
    {
        private readonly IConfiguration _config;
        public EmailSend(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmail2(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("promanuel415@gmail.com"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };



            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                emailClient.Authenticate(_config.GetSection("EmailUserName2").Value, _config.GetSection("EmailPassword2").Value); 
                emailClient.Send(email);
                emailClient.Disconnect(true);
            }
        }
    }
}
