using EzIdentity;
using Microsoft.AspNetCore.Builder;
using System.IO;

var builder = WebApplication
    .CreateBuilder();

builder.Services.AddEzIdentity();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseEzIdentityApi();
app.Run();
