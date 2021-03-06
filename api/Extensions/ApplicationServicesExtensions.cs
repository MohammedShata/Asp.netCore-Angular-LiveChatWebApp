using api.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Services;
using api.Helpers;
using api.SignalR;

namespace api.Extensions
{
    public static class ApplicationServicesExtensions
    {         
        
         public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,IConfiguration Config)
        {    services.AddSingleton<PresenceTracker>();   
            services.Configure<CloudinarySettings>(Config.GetSection("CloudinarySettings"));
              services.AddScoped<ITokenServices,TokenServices>();
              services.AddScoped<IPhotoServices, PhotoServices>();
              services.AddScoped<ILikesRepository,LikeRepository>();
              services.AddScoped<IMessageRepository,MessageRepository>();
              services.AddScoped<LogUserActivity>();
              services.AddScoped<IUserRepository,UserRepository>();
              services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options=>{
                 options.UseSqlite(Config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}