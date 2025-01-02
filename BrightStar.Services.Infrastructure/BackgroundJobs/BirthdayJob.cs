using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightStar.Services.Application.Common.Interfaces;

namespace BrightStar.Services.Infrastructure.BackgroundJobs
{
    public class BirthdayJob
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public BirthdayJob(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task ExecuteBirthDayMessageAsync()
        {
            var users = await _userService.GetUsersWithBirthdaysTodayAsync();
            if (users.Any())
            {
                await _notificationService.SendBirthdayMessagesAsync(users);
            }
        }
    }

}
