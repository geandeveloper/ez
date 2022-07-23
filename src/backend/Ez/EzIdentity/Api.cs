using EzIdentity.Events;
using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using EzIdentity.Extensions;
using EzIdentity.Features.RefreshToken;
using EzIdentity.Features.RevokeToken;

namespace EzIdentity;

public static class Api
{
    public static IApplicationBuilder UseEzIdentityApi(this WebApplication app)
    {
        app.UseCors("localhost");
        app.UsePathBase(new PathString("/api"));
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapPost("/users/token", async (
            [FromServices] LoginCommandHandler handler,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            LoginCommand command) =>
        {
            var eventStream = await handler.Handle(command, CancellationToken.None);

            var @event = eventStream.GetEvent<SucessLoginEvent>();
            httpContextAccessor.SetRefreshTokenAsCookie(@event.RefreshToken);

            return Results.Ok(new
            {
                UserId = @event.AggregateId,
                AccessToken = @event.AccessToken.Value,
                RefreshToken = @event.RefreshToken.Value
            });
        });

        app.MapPost("/users", async (
            [FromServices] CreateUserCommandHandler handler,
            CreateUserCommand command) =>
        {
            var eventStream = await handler.Handle(command, CancellationToken.None);
            return Results.Ok(eventStream);
        });

        app.MapPost("/users/refresh-token", async (
            [FromServices] RefreshTokenCommandHandler handler,
            IHttpContextAccessor httpContextAccessor
            ) =>
        {
            var eventStream = await handler.Handle(new RefreshTokenCommand(httpContextAccessor.GetRefreshTokenFromCookie()), CancellationToken.None);
            var @event = eventStream.GetEvent<SucessRenewTokenEvent>();
            httpContextAccessor.SetRefreshTokenAsCookie(@event.RefreshToken);

            return Results.Ok(new
            {
                UserId = @event.AggregateId,
                AccessToken = @event.AccessToken.Value,
                RefreshToken = @event.RefreshToken.Value
            });
        });

        app.MapPost("/users/revoke-token", async (
            [FromServices] RevokeTokenCommandHandler handler,
            IHttpContextAccessor httpContextAccessor
            ) =>
        {
            var eventStream = await handler.Handle(new RevokeTokenCommand(httpContextAccessor.GetRefreshTokenFromCookie()), CancellationToken.None);
            httpContextAccessor.DeleteRefreshCookie();

            return Results.Ok();
        });

        return app;

    }
}








