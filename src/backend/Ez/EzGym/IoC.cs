using EzGym.Features.Accounts.ChangeAvatar;
using EzGym.Features.Accounts.CreateAccount;
using EzGym.Features.Accounts.FollowAccount;
using EzGym.Features.Gyms.CreateGym;
using EzGym.Features.Profiles.UpInsertProfile;
using EzGym.Infra.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EzGym
{
    public static class IoC
    {
        public static IServiceCollection AddEzGym(this IServiceCollection services)
        {
            services.AddSingleton<IGymEventStore, GymEventStore>();
            services.AddSingleton<IGymQueryStore, GymEventStore>();

            
            services.AddSingleton<CreateAccountCommandHandler>();
            services.AddSingleton<CreateGymCommandHandler>();
            services.AddSingleton<ChangeAvatarCommandHandler>();
            services.AddSingleton<UpInsertProfileCommandHandler>();
            services.AddSingleton<FollowAccountCommandHandler>();

            return services;
        }
    }
}
