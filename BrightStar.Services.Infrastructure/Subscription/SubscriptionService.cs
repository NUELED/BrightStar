using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Infrastructure.Subscription
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubscriptionService(AppDbContext dbContext  /*, IHttpContextAccessor httpContextAccessor*/)
        {
            _dbContext = dbContext;
            //_httpContextAccessor = httpContextAccessor;
            _httpContextAccessor = new HttpContextAccessor();
        }


        public async Task<CustomResult<string>> SubscribeAsync(string phoneNumber, string username)
        {
           
            var userClaims = _httpContextAccessor.HttpContext?.User;
            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
            {
                return CustomResult<string>.Fail("Invalid Token", 401); 
            }

            
            var expirationClaim = userClaims.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
            if (expirationClaim != null && DateTime.UtcNow > DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim)).DateTime)
            {
                return CustomResult<string>.Fail("Token expired", 401); // Token expired
            }

            // Check if the user is already subscribed
            var existingSubscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber && s.Username == username);
            if (existingSubscription != null)
            {
                return CustomResult<string>.Fail("User is already subscribed", 409); // Already subscribed
            }

          
            var newSubscription = new AppSubscription
            {
                PhoneNumber = phoneNumber,
                Username = username,
                SubscriptionId = Guid.NewGuid().ToString(),
                SubscribedOn = DateTime.UtcNow
            };

            _dbContext.Subscriptions.Add(newSubscription);
            await _dbContext.SaveChangesAsync();

            return CustomResult<string>.Success(newSubscription.SubscriptionId, "User successfully subscribed", 200);
        }


        public async Task<CustomResult<bool>> CheckSubscriptionStatusAsync(string phoneNumber, string username)
        {
            var userClaims = _httpContextAccessor.HttpContext.User;
            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
            {
                return CustomResult<bool>.Fail("Invalid Token", 401); 
            }

            // Check subscription status
            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber && s.Username == username);

            if (subscription == null)
            {
                return CustomResult<bool>.Fail("User is not subscribed", 404); 
            }

            return CustomResult<bool>.Success(true, "User is subscribed", 200);
        }

    
        public async Task<CustomResult<bool>> UnsubscribeAsync(string phoneNumber, string username)
        {
            var userClaims = _httpContextAccessor.HttpContext.User;
            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
            {
                return CustomResult<bool>.Fail("Invalid Token", 401); 
            }

           
            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber && s.Username == username);

            if (subscription == null)
            {
                return CustomResult<bool>.Fail("User is not subscribed", 404); 
            }

            _dbContext.Subscriptions.Remove(subscription);
            await _dbContext.SaveChangesAsync();

            return CustomResult<bool>.Success(true, "User successfully unsubscribed", 200);
        }
    }

}
