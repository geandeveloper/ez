using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using EzIdentity.Users;

namespace EzIdentity.Services;

public static class TokenService
{
    public static TokenValidationParameters TokenValidationParameters(string tokenSecurityKey)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSecurityKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

        };
    }

    public static AccessToken GenerateAccessToken(Func<Claim[]> getClaims, string tokenSecurityKey, string issuerUrl)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecurityKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: issuerUrl,
            audience: issuerUrl,
            claims: getClaims(),
            expires: DateTime.Now.AddHours(24),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return new AccessToken(tokenString);
    }

    public static RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);
        return new RefreshToken(refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
    }
}

