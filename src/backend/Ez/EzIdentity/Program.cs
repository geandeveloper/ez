using EzCommon.Models;
using EzIdentity;
using EzIdentity.Events;
using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEzIdentityModule();

var app = builder.Build();

app.UseEzIdentityModule();



app.MapPost("/login", async ([FromServices] LoginCommandHandler handler, LoginCommand command) =>
{
    var eventStream = await handler.Handle(command, CancellationToken.None);

    return Results.Ok(FromEventStream<SucessLoginEvent, dynamic>(eventStream, @event => new
    {
        AccessToken = @event.AccessToken,

    }));
});

app.MapPost("/users", [Authorize] async ([FromServices] CreateUserCommandHandler handler, CreateUserCommand command) =>
{
    var eventStream = await handler.Handle(command, CancellationToken.None);
    return Results.Ok(eventStream);
});



app.Run();

static TResult FromEventStream<TEvent, TResult>(EventStream eventStream, Func<TEvent, TResult> parse) where TEvent : class
{
    var eventName = typeof(TEvent).Name;
    var @event = eventStream.EventRows.Where(@event => @event.EventName == eventName).FirstOrDefault();
    return parse(@event.Data as TEvent);
}

