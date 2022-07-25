using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Http;
using EzIdentity.Extensions;
using EzIdentity.Features.RefreshToken;

namespace EzIdentity;

public static class IoC
{
    public static IServiceCollection AddEzIdentity(this IServiceCollection services)
    {

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IEventStore, EventStoreInLocal>();

        services.AddMediatR(typeof(IoC));
        services.AddSingleton<IBus, InMemoryBus>();

        services.AddSingleton<LoginCommandHandler>();
        services.AddSingleton<CreateUserCommandHandler>();
        services.AddSingleton<RefreshTokenCommandHandler>();
        services.AddSingleton<RevokeTokenCommandHandler>();

        services.AddAuthWithJwtBearer();

        return services;
    }
}

