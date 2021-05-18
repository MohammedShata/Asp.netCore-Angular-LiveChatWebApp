using api.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Services;
using api.Helpers;

namespace api.Extensions
{
    public static class ApplicationServicesExtensions
    {         
        
         public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,IConfiguration Config)
        {       services.Configure<CloudinarySettings>(Config.GetSection("CloudinarySettings"));
              services.AddScoped<ITokenServices,TokenServices>();
              services.AddScoped<IPhotoServices, PhotoServices>();
              services.AddScoped<IUserRepository,UserRepository>();
              services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options=>{
                 options.UseSqlite(Config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}