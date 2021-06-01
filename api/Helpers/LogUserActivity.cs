using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Data.Migrations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace api.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext =await next();
            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var username=resultContext.HttpContext.User.GetUsername();
            var repo=resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user= await repo.GetUserByUsernameAsync(username);
            user.LastActive=DateTime.Now;
            await repo.SaveAllAsync();
        }
    }
}