using Microsoft.AspNetCore.Authentication;

namespace Common.Authentication.ApiKeyAuthentication
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {

        public string ApiKey { get; set; }
        public string ApiKeyHeaderName { get; set; }

        public const string DefaultScheme = "ApiKey";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
