using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BrightStar.Services.Infrastructure.BackgroundJobs
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
           _context = context;
        }


        public async Task<List<AppUser>> GetUsersWithBirthdaysTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            //return await _context.AppUsers.Where(u => u.DateOfBirth.Day == today.Day && u.DateOfBirth.Month == today.Month).ToListAsync();
            return await _context.AppUsers.Where(u => u.DateOfBirth == today).ToListAsync();
        }


    }
}
