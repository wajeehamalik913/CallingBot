using Microsoft.AspNetCore.Authentication;

namespace Common.Authentication.BearerAuthentication
{
    public class BearerAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Bearer";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
