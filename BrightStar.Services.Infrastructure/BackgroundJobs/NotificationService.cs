using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;

namespace BrightStar.Services.Infrastructure.BackgroundJobs
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailSend _emailService;
        public NotificationService(IEmailSend emailService)
        {
            _emailService = emailService; 
        }
        public async Task SendBirthdayMessagesAsync(List<AppUser> users)
        {
            foreach (var user in users)
            {
                var emailRequest = new EmailDto
                {
                    To = user.Email,
                    Subject = "Happy Birthday!",
                    Body = $"Happy Birthday, {user.Name}! We wish you a fantastic day!"
                };
                await _emailService.SendEmail2(emailRequest);
            }
        }

    }
}
