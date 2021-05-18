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
namespace api.Extensions
{
    public  static class IdentityServicesExtensions
    {
         public static IServiceCollection AddIdentityServices(
            this IServiceCollection services,IConfiguration Config)
        
        {
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
            });
            return services;

        }
        
    }
}