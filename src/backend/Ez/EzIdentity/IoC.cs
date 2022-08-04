using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using EzIdentity.Extensions;
using EzIdentity.Features.RefreshToken;
using EzIdentity.Infra.Storage;

namespace EzIdentity;

public static class IoC
{
    public static IServiceCollection AddEzIdentity(this IServiceCollection services)
    {

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IIdentityEventStore, IdentityEventStore>();
        services.AddSingleton<IIdentityQueryStore, IdentityEventStore>();

        services.AddSingleton<LoginCommandHandler>();
        services.AddSingleton<CreateUserCommandHandler>();
        services.AddSingleton<RefreshTokenCommandHandler>();
        services.AddSingleton<RevokeTokenCommandHandler>();

        services.AddAuthWithJwtBearer();

        return services;
    }
}

