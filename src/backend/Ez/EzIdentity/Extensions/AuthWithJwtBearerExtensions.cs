using EzIdentity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace EzIdentity.Extensions;

public static class AuthWithJwtBearerExtensions
{
    public static IServiceCollection AddAuthWithJwtBearer(this IServiceCollection services)
    {

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
