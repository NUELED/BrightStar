using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Application.Common.Utility;
using BrightStar.Services.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public DbInitializer(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }


                if (!_roleManager.RoleExistsAsync(SD.RoleAdmin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin)).Wait();
                    _roleManager.CreateAsync(new IdentityRole(SD.RoleParticipant)).Wait();
                    _roleManager.CreateAsync(new IdentityRole(SD.RoleOrganizer)).Wait();


                    _userManager.CreateAsync(new AppUser
                    {
                        UserName = "admin@bright_star.com",
                        Email = "admin@bright_star.com",
                        Name = "Nuel Ed",
                        NormalizedUserName = "ADMIN@BRIGHT_STAR.COM",
                        NormalizedEmail = "ADMIN@BRIGHT_STAR.COM",
                        PhoneNumber = "1123456890",

                    }, "Password123*").GetAwaiter().GetResult();

                    AppUser user = _context.AppUsers.FirstOrDefault(u => u.Email == "admin@bright_star.com");
                    _userManager.AddToRoleAsync(user, SD.RoleAdmin).GetAwaiter().GetResult();
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
