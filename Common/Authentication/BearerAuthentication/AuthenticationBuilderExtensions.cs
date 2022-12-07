using Microsoft.AspNetCore.Authentication;
using System;

namespace Common.Authentication.BearerAuthentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddBearerSupport(
            this AuthenticationBuilder builder,
            string scheme,
            Action<BearerAuthenticationOptions> options
            )
        {
            // TODO Update to register token service here to support multiple services
            return builder.AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>(scheme, options);
        }
    }
}
