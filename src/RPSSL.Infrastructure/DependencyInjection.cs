using Polly;
using Polly.Extensions.Http;
using RPSSL.Infrastructure.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RPSSL.Infrastructure.Authentication;
using RPSSL.Application.Abstractions.Data;
using RPSSL.Application.Abstractions.Clients;
using Microsoft.Extensions.DependencyInjection;
using RPSSL.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RPSSL.Infrastructure.ExternalServices.RandomNumberClient;

namespace RPSSL.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        services.AddRandomNumberHttpClient();

        services.ConfigureAuthentication();

        services.AddAuthorization();

        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
    }

    private static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.ConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer();

        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }

    private static void AddRandomNumberHttpClient(this IServiceCollection services)
    {
        services.AddOptions<RandomNumberApiOptions>()
            .BindConfiguration(RandomNumberApiOptions.ConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var retryCount = 3;

        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

        services.AddHttpClient<IRandomNumberApiClient, RandomNumberApiClient>((serviceProvider, httpClient) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<RandomNumberApiOptions>>().Value;
            httpClient.BaseAddress = new Uri(settings.Url);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromSeconds(5),
        })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
        .AddPolicyHandler(retryPolicy)
        .AddPolicyHandler(circuitBreakerPolicy);
    }
}
