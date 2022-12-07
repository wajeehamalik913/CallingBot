using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Common.Authentication.BasicAuthentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddBasicSupport(
            this AuthenticationBuilder builder,
            string Scheme,
            Action<BasicAuthenticationOptions> options)
        {
            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<BasicAuthenticationOptions>,
                BasicAuthenticationPostConfigureOptions>());
            return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(Scheme, options);
        }

    }
}
