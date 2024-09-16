using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using BrightStar.Services.Infrastructure.Jwt_Auth;
using BrightStar.Services.Infrastructure.Subscription;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace BrightStar.Services.SubscribeAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
       

        public static WebApplicationBuilder AddOtherServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            //builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<ITokenProvider, TokenProvider>();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bright", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Bearer and then token in the field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                     },
                      new string[] { }
                   }
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddDefaultTokenProviders()
                            .AddEntityFrameworkStores<AppDbContext>();

            var apiSettingsSection = builder.Configuration.GetSection("APISettings:JwtOptions");
            builder.Services.Configure<JwtOptions>(apiSettingsSection);

            var appSetiings = apiSettingsSection.Get<JwtOptions>();
            var key = Encoding.ASCII.GetBytes(appSetiings.Secret);

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = appSetiings.Issuer,
                    ValidAudience = appSetiings.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return builder;
        }


    }
}
