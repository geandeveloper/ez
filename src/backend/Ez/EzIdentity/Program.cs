using EzIdentity;
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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEzIdentityModule();


var app = builder.Build();
app.UseEzIdentityModule();




app.MapPost("/login", async (
    [FromServices] LoginCommandHandler handler,
    [FromServices] IHttpContextAccessor httpContextAccessor,
    LoginCommand command) =>
{
    var eventStream = await handler.Handle(command, CancellationToken.None);

    var @event = eventStream.GetEvent<SucessLoginEvent>();
    httpContextAccessor.SetRefreshTokenAsCookie(@event.RefreshToken);

    return Results.Ok(new
    {
        AccessToken = @event.AccessToken.Value,
        RefreshToken = @event.RefreshToken.Value
    });
});

app.MapPost("/users",
    [Authorize]
async (
    [FromServices] CreateUserCommandHandler handler,
    CreateUserCommand command) =>
{
    var eventStream = await handler.Handle(command, CancellationToken.None);
    return Results.Ok(eventStream);
});

app.MapPost("/users/{userId}/refresh-token", async (
    [FromServices] RefreshTokenCommandHandler handler,
    IHttpContextAccessor httpContextAccessor,
    string userId) =>
{
    var eventStream = await handler.Handle(new RefreshTokenCommand(userId.ToGuid(), httpContextAccessor.GetRefreshTokenFromCookie()), CancellationToken.None);
    var @event = eventStream.GetEvent<SucessRenewTokenEvent>();
    return Results.Ok(new
    {
        AccessToken = @event.AccessToken.Value,
        RefreshToken = @event.RefreshToken.Value
    });
});



app.Run();

