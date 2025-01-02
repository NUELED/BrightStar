using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightStar.Services.Domain.Entities;

namespace BrightStar.Services.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<List<AppUser>> GetUsersWithBirthdaysTodayAsync();
    }
}
