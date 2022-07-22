using EzIdentity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

var builder = WebApplication
    .CreateBuilder();

//builder.Services.AddEzIdentity();

builder.Services.AddControllersWithViews();
var app = builder.Build();

//app.UseEzIdentityApi();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.Run();
