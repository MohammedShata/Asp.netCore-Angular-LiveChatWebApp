using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using api.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class Seed {
        public static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        if(await userManager.Users.AnyAsync()) return;

        var userData= await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
        var users= JsonSerializer.Deserialize<List<AppUser>>(userData);
        foreach(var user in users)
        {
        //    using var hmac=new HMACSHA512();
           user.UserName=user.UserName.ToLower();
        //    user.PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
        //    user.PasswordSalt=hmac.Key;
           

          await  userManager.CreateAsync(user,"Pa$$w0rd");
        }
    

 }
    }
}