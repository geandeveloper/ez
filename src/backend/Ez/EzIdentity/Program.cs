using EZGym.Manager.Identity;
using EzIdentity.Features.Login;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEzIdentityModule();

var app = builder.Build();

app.UseEzIdentityModule();



app.MapPost("/login", ([FromServices] LoginCommandHandler handler, LoginCommand command) =>
{
    handler.Handle(command, CancellationToken.None);

    return Results.Ok();
});


app.Run();
