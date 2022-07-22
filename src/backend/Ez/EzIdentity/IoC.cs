using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using EzIdentity.Infra.Bus;
using EzIdentity.Infra.Storage;
using EzIdentity.Services;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Http;
using EzIdentity.Extensions;
using EzIdentity.Features.RefreshToken;
using Microsoft.AspNetCore.Builder;

namespace EzIdentity;

public static class IoC
{
    public static IServiceCollection AddEzIdentity(this IServiceCollection services)
    {

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IEventStore, EventStoreInLocal>();

        services.AddMediatR(typeof(IoC));
        services.AddSingleton<IBus, InMemoryBus>();

        services.AddSingleton<TokenService>();
        services.AddSingleton<LoginCommandHandler>();
        services.AddSingleton<CreateUserCommandHandler>();
        services.AddSingleton<RefreshTokenCommandHandler>();

        services.AddCors(c =>
        {
            c.AddPolicy("localhost", options => options.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());
        });

        services.AddAuthWithJwtBearer();

        return services;
    }
}

