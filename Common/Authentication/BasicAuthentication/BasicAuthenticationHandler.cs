using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Common.Authentication.BasicAuthentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly ICredentialRepository _credentialRepository;
        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ICredentialRepository credentialRepository) : base(options, logger, encoder, clock)
        {
            this._credentialRepository = credentialRepository;
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
            if (authSplit.Length != 2 || authSplit[0] != "Basic")
            {
                return AuthenticateResult.Fail("Auth invalid");
            }

            var creds = Encoding.UTF8.GetString(Convert.FromBase64String(authSplit[1])).Split(':');
            if (creds.Length != 2)
            {
                return AuthenticateResult.Fail("Auth invalid");
            }

            // - Validating credentials
            var userPrincipal = await this._credentialRepository.ValidateCredentialsAsync(
                username: creds[0],
                password: creds[1],
                Options.Scheme);

            if (userPrincipal != null)
            {
                var ticket = new AuthenticationTicket(userPrincipal, Options.Scheme);
                return AuthenticateResult.Success(ticket);
            }
            return AuthenticateResult.Fail("Credentials invalid");
        }
    }
}
