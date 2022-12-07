using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.Authentication.BearerAuthentication
{
    public interface ITokenService
    {
        public Task<ClaimsPrincipal> ValidateTokenAsync(string token);
    }
}
