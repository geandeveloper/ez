using Microsoft.AspNetCore.Builder;
using EzCommon;
using EzIdentity;
using Microsoft.AspNetCore.Http;
using EzGym;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication
    .CreateBuilder();


builder.Services.AddCors(c =>
        {
            c.AddPolicy("localhost", options => options
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
        });

//EzGym Services
builder.Services
    .AddEzCommon(typeof(EzCommon.Api), typeof(EzGym.Api), typeof(EzIdentity.Api))
    .AddEzIdentity()
    .AddEzGym();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("localhost");
app.UsePathBase(new PathString("/api"));
app.UseRouting();


//EzGym Modules
app.UseEzCommonApi()
.UseEzIdentityApi()
.UseEzGymApi();

app.Run();
