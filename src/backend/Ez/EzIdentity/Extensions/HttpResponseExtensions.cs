using EzIdentity.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace EzIdentity.Extensions
{
    public static class HttpExtensions
    {
        public static void SetRefreshTokenAsCookie(this IHttpContextAccessor httpContextAccessor, RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions {
                HttpOnly = true,
                Secure = true,
                Expires = refreshToken.Expires 
            };

            httpContextAccessor.HttpContext.Response.Cookies.Append(nameof(RefreshToken), refreshToken.Value, cookieOptions);
        }

        public static string GetRefreshTokenFromCookie(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Request.Cookies[nameof(RefreshToken)].ToString();
        }
    }
}
