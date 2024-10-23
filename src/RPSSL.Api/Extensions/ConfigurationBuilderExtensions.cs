namespace RPSSL.Api.Extensions;

internal static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAppConfigurations(this IConfigurationBuilder builder, IWebHostEnvironment environment)
    {
        return builder
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
    }
}
