using api.Entites;
using Microsoft.IdentityModel.Tokens;

namespace api.Interfaces
{
    public interface ITokenServices
    {
       
        string CreateToken (AppUser user);
    }
}