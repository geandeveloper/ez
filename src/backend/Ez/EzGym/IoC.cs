using EzCommon;
using EzGym.Features.Gyms.CreateGym;
using EzIdentity;
using Microsoft.Extensions.DependencyInjection;

namespace EzGym
{
    public static class IoC
    {
        public static IServiceCollection AddEzGym(this IServiceCollection services)
        {

            services.AddCors(c =>
                    {
                        c.AddPolicy("localhost", options => options
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
                    });


            services.AddSingleton<CreateGymCommandHandler>();

            return services;
        }
    }
}
