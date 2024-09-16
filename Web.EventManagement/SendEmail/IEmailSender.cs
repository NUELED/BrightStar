using BrightStar.Services.Application.Common.DTO;

namespace Web.EventManagement.SendEmail
{
    public interface IEmailSender
    {

        Task SendEmail(EmailDto request);
        Task SendEmail2(EmailDto request);
    }
}
