using EzIdentity;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEzIdentity();
var app = builder.Build();


app.UseEzIdentityApi();

app.Run();
