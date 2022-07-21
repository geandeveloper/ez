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

namespace EzIdentity;

public static class EZIdentityModule
{
    public static IServiceCollection AddEzIdentityModule(this IServiceCollection services)
    {

        services.AddSingleton<TokenService>();
        services.AddSingleton<LoginCommandHandler>();
        services.AddSingleton<CreateUserCommandHandler>();


        services.AddSingleton<IEventStore, EventStoreInLocal>();

        services.AddMediatR(typeof(EZIdentityModule));
        services.AddSingleton<IBus, InMemoryBus>();

        var key = Encoding.ASCII.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE");

        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }

    public static IApplicationBuilder UseEzIdentityModule(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

}

