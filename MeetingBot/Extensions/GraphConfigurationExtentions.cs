using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;

namespace MeetingBot.Extensions
{
    public static class GraphConfigurationExtentions
    {
        

            public static IServiceCollection ConfigureGraphComponent(this IServiceCollection services, Action<AzureAdOptions> azureAdOptionsAction)
            {
                var options = new AzureAdOptions();
                azureAdOptionsAction(options);
                services.AddSingleton(options);

                IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(options.ClientId)
                .WithTenantId(options.TenantId)
                .WithClientSecret(options.ClientSecret)
                .Build();

                ClientCredentialProvider authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);

                services.AddSingleton<GraphServiceClient>(sp =>
                {
                    return new GraphServiceClient(authenticationProvider);
                });

                return services;
            }
        
    }
}

