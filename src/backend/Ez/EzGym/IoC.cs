using EzGym.Features.Accounts.CreateAccount;
using EzGym.Features.Gyms.CreateGym;
using EzGym.Features.Profiles.ChangeAvatar;
using EzGym.Infra.Storage;
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


            services.AddSingleton<IGymEventStore, GymEventStore>();
            services.AddSingleton<IGymQueryStorage, GymEventStore>();
            services.AddSingleton<CreateAccountCommandHandler>();
            services.AddSingleton<CreateGymCommandHandler>();
            services.AddSingleton<ChangeAvatarCommandHandler>();

            return services;
        }
    }
}
