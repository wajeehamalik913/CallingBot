namespace Common
{
    public class Constants
    {
        public const string ClientIdConfigurationSettingsKey = "AzureAd:ClientId";

        public const string TenantIdConfigurationSettingsKey = "AzureAd:TenantId";

        public const string ClientSecretConfigurationSettingsKey = "AzureAd:ClientSecret";

        public const string UserIdConfigurationSettingsKey = "UserId";

        public const string MicrosoftAppPasswordConfigurationSettingsKey = "MicrosoftAppPassword";

        public const string MicrosoftAppIdConfigurationSettingsKey = "MicrosoftAppId";

        public const string BotBaseUrlConfigurationSettingsKey = "BotBaseUrl";

        public const string DatabaseConnectionStringConfigurationSettingsKey = "DatabaseConnectionString";

        public const string MassTransitHostAddressConfigurationSettingsKey = "MassTransit:HostAddress";
        public const string MassTransitUsernameConfigurationSettingsKey = "MassTransit:Username";
        public const string MassTransitPasswordConfigurationSettingsKey = "MassTransit:Password";

        public const string MassTransitEndpointConfigurationSettingsKey = "MassTransit:Endpoint";

        public const string RedisConnectionStringConfigurationSettingsKey = "Redis:ConnectionString";

        public const string MassTransitWorkerExchange = "muk-worker";
        public const string MassTransitDeadletterExchange = "muk-deadletter";
        public const string MassTransitDeadletterQueue = "muk-deadletter";

        public const string PretzBaseUrl = "Pretz:BaseUrl";
        public const string PretzApiKey = "Pretz:ApiKey";
        public const string PretzDisable = "Pretz:Disable";
    }
}
