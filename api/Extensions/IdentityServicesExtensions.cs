using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using api.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace api.Extensions
{
    public  static class IdentityServicesExtensions
    {
         public static IServiceCollection AddIdentityServices(
            this IServiceCollection services,IConfiguration Config)
        
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric=false;
            })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddEntityFrameworkStores<DataContext>();
            
              services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>{
                options.TokenValidationParameters=new 
                TokenValidationParameters
                {
                  ValidateIssuerSigningKey=true,
                  IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["TokenKey"])),
                     ValidateIssuer=false,
                     ValidateAudience=false
                };
                options.Events=new JwtBearerEvents
               {
                   OnMessageReceived= context=>
                   { 
                       var accessToken=context.Request.Query["access_token"];
                       var path=context.HttpContext.Request.Path;
                       if(!string.IsNullOrEmpty(accessToken)&& path.StartsWithSegments("/hubs"))
                       {
                       context.Token=accessToken;
                       }
                        return Task.CompletedTask;
                   }
                 

               };
            });
               
            services.AddAuthorization(opt=>
            {
                opt.AddPolicy("RequireAdminRole",policy=>policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratePhotoRole",policy=>policy.RequireRole("Admin","Moderator"));
                });
            return services;

        }
        
    }
}