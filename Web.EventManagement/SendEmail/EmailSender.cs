using BrightStar.Services.Application.Common.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace Web.EventManagement.SendEmail
{
    public class EmailSender : IEmailSender
    {

        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {

            _config = config;
        }

        public async Task SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("mozelle.kuhlman48@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
            try
            {
                using var smtp = new SmtpClient();

                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (SmtpCommandException ex)
            {
                // Handle SMTP command errors such as authentication failure or connection issues
                Console.WriteLine($"SMTP Command Error: {ex.Message}");
            }
            catch (SmtpProtocolException ex)
            {
                
                Console.WriteLine($"SMTP Protocol Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"General Error: {ex.Message}");
            }
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
                emailClient.Authenticate(_config.GetSection("EmailUserName2").Value, _config.GetSection("EmailPassword2").Value); //
                emailClient.Send(email);
                emailClient.Disconnect(true);
            }
        }
    }
}
