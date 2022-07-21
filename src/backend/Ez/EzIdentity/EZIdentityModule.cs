using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using EzIdentity.Infra.Bus;
using EzIdentity.Infra.Storage;
using EzIdentity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using EzIdentity.Extensions;
using EzIdentity.Features.RefreshToken;

namespace EzIdentity;

public static class EZIdentityModule
{
    public static IServiceCollection AddEzIdentityModule(this IServiceCollection services)
    {

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddSingleton<TokenService>();
        services.AddSingleton<LoginCommandHandler>();
        services.AddSingleton<CreateUserCommandHandler>();
        services.AddSingleton<RefreshTokenCommandHandler>();


        services.AddSingleton<IEventStore, EventStoreInLocal>();

        services.AddMediatR(typeof(EZIdentityModule));
        services.AddSingleton<IBus, InMemoryBus>();

        services.AddAuthWithJwtBearer();

        return services;
    }

    public static IApplicationBuilder UseEzIdentityModule(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

}

