using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using EzIdentity.Extensions;
using EzIdentity.Features.RefreshToken;
using EzIdentity.Infra.Storage;
using EzCommon.Events;
using EzIdentity.Events;
using EzIdentity.Features.RevokeToken;
using EzIdentity.Models;

namespace EzIdentity;

public static class IoC
{
    public static IServiceCollection AddEzIdentity(this IServiceCollection services, IEventRegister eventRegister)
    {

        services.AddScoped<IIdentityEventStore, IdentityEventStore>();
        services.AddScoped<IIdentityQueryStore, IdentityEventStore>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddTransient<LoginCommandHandler>();
        services.AddTransient<CreateUserCommandHandler>();
        services.AddTransient<RefreshTokenCommandHandler>();
        services.AddTransient<RevokeTokenCommandHandler>();

        services.AddAuthWithJwtBearer();

        eventRegister
            .Register<UserCreatedEvent>()
            .Register<SucessRevokeTokenEvent>()
            .Register<SucessRenewTokenEvent>()
            .Register<SucessLoginEvent>()
            .Register<SnapShotEvent<User>>();

        return services;
    }
}

