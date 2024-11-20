namespace SkillNet.Notifications.Config
{
    internal static class SecurityConfig
    {
        // Set appropriately for you app. Considering moving into configuration.
        internal static string[] ValidIssuers = ["https://dev-siayq52c1jb0k38w.us.auth0.com/"];
        internal static string[] ValidAudiences = ["https://localhost:7091"];
        internal static string OpenIdConnectConfigurationUrl = "https://dev-siayq52c1jb0k38w.us.auth0.com/v2.0/.well-known/openid-configuration";
    }
}
