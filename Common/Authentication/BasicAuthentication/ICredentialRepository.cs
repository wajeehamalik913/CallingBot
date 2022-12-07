using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.Authentication.BasicAuthentication
{
    public interface ICredentialRepository
    {
        Task<ClaimsPrincipal> ValidateCredentialsAsync(string username, string password, string authType);
    }
}
