using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using EzIdentity.Extensions;
using EzIdentity.Features.RefreshToken;
using EzIdentity.Infra.Storage;
using EzCommon.Events;
using EzIdentity.Events;
using EzIdentity.Models;
using EzIdentity.SnapShots;

namespace EzIdentity;

public static class IoC
{
    public static IServiceCollection AddEzIdentity(this IServiceCollection services, IEventRegister eventRegister)
    {

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IIdentityEventStore, IdentityEventStore>();
        services.AddSingleton<IIdentityQueryStore, IdentityEventStore>();

        services.AddSingleton<LoginCommandHandler>();
        services.AddSingleton<CreateUserCommandHandler>();
        services.AddSingleton<RefreshTokenCommandHandler>();
        services.AddSingleton<RevokeTokenCommandHandler>();

        services.AddAuthWithJwtBearer();

        eventRegister
            .Register<UserCreatedEvent>()
            .Register<SucessRevokeTokenEvent>()
            .Register<SucessRenewTokenEvent>()
            .Register<SucessLoginEvent>()
            .Register<SnapShotEvent<UserSnapShot>>();

        return services;
    }
}

