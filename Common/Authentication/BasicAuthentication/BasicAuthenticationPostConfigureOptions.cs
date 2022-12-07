using Microsoft.Extensions.Options;

namespace Common.Authentication.BasicAuthentication
{
    public class BasicAuthenticationPostConfigureOptions : IPostConfigureOptions<BasicAuthenticationOptions>
    {
        public void PostConfigure(string name, BasicAuthenticationOptions options)
        { }
    }
}
