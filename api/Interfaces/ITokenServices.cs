using System.Threading.Tasks;
using api.Entites;
using Microsoft.IdentityModel.Tokens;

namespace api.Interfaces
{
    public interface ITokenServices
    {
       
     Task<string> CreateToken (AppUser user);
    }
}