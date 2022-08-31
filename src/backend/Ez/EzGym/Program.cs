using Microsoft.AspNetCore.Builder;
using EzCommon;
using EzIdentity;
using Microsoft.AspNetCore.Http;
using EzGym;
using EzPayment;
using EzPayment.Apis;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication
    .CreateBuilder();

//EzGym Services

builder.Services.AddCors(c =>
        {
            c.AddPolicy("frontend", options => options
            .WithOrigins(builder.Configuration["EzGymSettings:Frontend:EzGymWebUrl"])
            .AllowAnyHeader()
            .AllowAnyMethod());
        });


builder.Services.Configure<EzGymSettings>(builder.Configuration.GetSection("EzGymSettings"));
builder.Services.Configure<EzIdentitySettings>(builder.Configuration.GetSection("EzIdentitySettings"));
builder.Services.Configure<EzPaymentSettings>(builder.Configuration.GetSection("EzPaymentSettings"));


builder.Services
    .AddEzCommon(
        typeof(EzCommon.Api),
        typeof(EzGym.Api),
        typeof(EzIdentity.Api),
        typeof(EzPayment.IoC)
        )
    .AddEzIdentity(builder.Configuration)
    .AddEzGym(builder.Configuration)
    .AddEzPayment(builder.Configuration);


var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("frontend");
app.UsePathBase(new PathString("/api"));
app.UseRouting();


//EzGym Modules
app.UseEzCommonApi()
    .UseEzIdentityApi()
    .UseEzGymApi()
    .UseEzPaymentApi();

app.Run();
