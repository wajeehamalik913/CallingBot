using Microsoft.AspNetCore.Authentication;

namespace Common.Authentication.BasicAuthentication
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Basic";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
