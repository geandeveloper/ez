using EzIdentity.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EzIdentity.Services;

public static class TokenService
{

    public static TokenValidationParameters TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };

    public static AccessToken GenerateAccessToken(Func<Claim[]> getClaims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE"));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: "https://localhost:5000",
            audience: "https://localhost:5000",
            claims: getClaims(),
            expires: DateTime.Now.AddHours(24),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return new AccessToken(tokenString);
    }

    public static RefreshToken GenereateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);
        return new RefreshToken(refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
    }
 }

