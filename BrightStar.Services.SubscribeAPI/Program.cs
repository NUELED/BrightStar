using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using BrightStar.Services.Infrastructure.Jwt_Auth;
using BrightStar.Services.Infrastructure.Subscription;
using BrightStar.Services.SubscribeAPI.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.AddOtherServices();
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("BrightConnect")!, name : "Sql Health");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BrightConnect")));

builder.Services.AddSwaggerGen();
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddPolicy("FixedWindowPolicy", context =>
//        RateLimitPartition.GetFixedWindowLimiter(partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "global", partition =>
//            new FixedWindowRateLimiterOptions
//            {
//                PermitLimit = 5,         // Allows 5 requests
//                Window = TimeSpan.FromSeconds(10), // Every 10 seconds
//                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
//                QueueLimit = 2            // Maximum 2 requests in queue
//            }));

//    // Customize other policies or add more here as needed
//});
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.MapHealthChecks("/health");
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

