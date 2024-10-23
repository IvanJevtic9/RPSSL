using FluentValidation;
using RPSSL.Domain.Players;
using RPSSL.Application.Game;
using Microsoft.AspNetCore.Identity;
using RPSSL.Application.Abstractions.Service;
using Microsoft.Extensions.DependencyInjection;
using RPSSL.Application.Abstractions.Behaviors;

namespace RPSSL.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        services.AddScoped<IPasswordHasher<Player>, PasswordHasher<Player>>();

        services.AddScoped<IGameService, GameService>();

        return services;
    }    
}
