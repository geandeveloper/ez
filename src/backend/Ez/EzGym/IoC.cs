using EzCommon.BackgroundServices;
using EzCommon.Events;
using EzGym.Events;
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
        public static IServiceCollection AddEzGym(this IServiceCollection services, IEventRegister eventRegister)
        {
            services.AddSingleton<IGymEventStore, GymEventStore>();
            services.AddSingleton<IGymQueryStore, GymEventStore>();

            services.AddSingleton<CreateAccountCommandHandler>();
            services.AddSingleton<CreateGymCommandHandler>();
            services.AddSingleton<ChangeAvatarCommandHandler>();
            services.AddSingleton<UpInsertProfileCommandHandler>();
            services.AddSingleton<FollowAccountCommandHandler>();

            services.AddHostedService<KafkaConsumerBackgroundService>();

            eventRegister
                .Register<AccountCreatedEvent>()
                .Register<AddedFollowAccountEvent>()
                .Register<AvatarImageAccountChangedEvent>()
                .Register<GymCreatedEvent>()
                .Register<ProfileChangedEvent>()
                .Register<RemovedFollowAccountEvent>()
                .Register<StartFollowAccountEvent>()
                .Register<UnfollowAccountEvent>();

            return services;
        }
    }
}
