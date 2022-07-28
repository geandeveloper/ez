using Microsoft.AspNetCore.Builder;
using EzCommon;
using EzIdentity;
using Microsoft.AspNetCore.Http;
using EzGym;

var builder = WebApplication
    .CreateBuilder();


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
