using EzCommon.Models;
using EzIdentity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace EzIdentity.Extensions;

public static class AuthWithJwtBearerExtensions
{
    public static IServiceCollection AddAuthWithJwtBearer(this IServiceCollection services)
    {

        services.AddScoped((context) =>
        {
            var httpAccessor = context.GetService<IHttpContextAccessor>();
            var user = httpAccessor.HttpContext.User;

            return new EzPrincipal(
                id: user.Claims.First(c => c.Type == nameof(EzPrincipal.Id)).Value,
                name: user.Claims.First(c => c.Type == ClaimTypes.Name).Value,
                userName: user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
                email: user.Claims.First(c => c.Type == ClaimTypes.Email).Value
                );
        });

        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = TokenService.TokenValidationParameters;
        });

        return services;
    }
}
