using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Common.Authentication.BearerAuthentication
{
    public class BearerAuthenticationHandler : AuthenticationHandler<BearerAuthenticationOptions>
    {
        private readonly ITokenService _tokenService;

        public BearerAuthenticationHandler(
            IOptionsMonitor<BearerAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenService tokenService
            ) : base(options, loggerFactory, encoder, clock)
        {
            this._tokenService = tokenService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // - Validating if header exists
            if (!Request.Headers.TryGetValue("Authorization", out var authValues))
            {
                return AuthenticateResult.Fail("No auth provided");
            }

            var providedAuth = authValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(providedAuth))
            {
                return AuthenticateResult.Fail("No auth provided");
            }

            // - Ensuring format is correct
            var authSplit = providedAuth.Split(' ');
            if (authSplit.Length != 2 || authSplit[0] != "Bearer")
            {
                return AuthenticateResult.Fail("Auth invalid");
            }

            var userPrincipal = await this._tokenService.ValidateTokenAsync(authSplit[1]);

            if (userPrincipal != null)
            {
                var ticket = new AuthenticationTicket(userPrincipal, Options.Scheme);
                return AuthenticateResult.Success(ticket);
            }
            return AuthenticateResult.Fail("Credentials invalid");

        }
    }
}
