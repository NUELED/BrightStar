using BrightStar.Services.Application.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.Interfaces
{
    public interface ISubscriptionService
    {
        Task<CustomResult<string>> SubscribeAsync(string phoneNumber, string username);
        Task<CustomResult<bool>> CheckSubscriptionStatusAsync(string phoneNumber, string username);
        Task<CustomResult<bool>> UnsubscribeAsync(string phoneNumber, string username);

    }
}
