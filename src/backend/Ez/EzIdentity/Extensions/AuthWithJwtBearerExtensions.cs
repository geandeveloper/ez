using EzCommon.Models;
using EzIdentity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace EzIdentity.Extensions;

public static class AuthWithJwtBearerExtensions
{
    public static IServiceCollection AddAuthWithJwtBearer(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddScoped(context =>
        {
            var httpAccessor = context.GetRequiredService<IHttpContextAccessor>();
            var user = httpAccessor.HttpContext!.User;

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
            options.TokenValidationParameters = TokenService.TokenValidationParameters(configuration["EzIdentitySettings:TokenSecurityKey"]);
        });

        return services;
    }

}
