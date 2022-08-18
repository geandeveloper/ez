using EzCommon.Consumers;
using EzCommon.Events;
using EzGym.Events;
using EzGym.Features.Accounts.ChangeAvatar;
using EzGym.Features.Accounts.CreateAccount;
using EzGym.Features.Accounts.FollowAccount;
using EzGym.Features.Accounts.UnfollowAccount;
using EzGym.Features.Accounts.UpdateWallet;
using EzGym.Features.Accounts.UpInsertAccountProfile;
using EzGym.Features.Gyms.CreateGym;
using EzGym.Infra.Storage;
using EzGym.Models;
using EzIdentity.Models;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace EzGym
{
    public static class IoC
    {
        public static IServiceCollection AddEzGym(this IServiceCollection services, IEventRegister eventRegister)
        {
            services.AddScoped<IGymEventStore, GymEventStore>();
            services.AddScoped<IGymQueryStore, GymEventStore>();

            services.AddTransient<CreateAccountCommandHandler>();
            services.AddTransient<CreateGymCommandHandler>();
            services.AddTransient<ChangeAvatarCommandHandler>();
            services.AddTransient<UpInsertAccountProfileCommandHandler>();
            services.AddTransient<FollowAccountCommandHandler>();
            services.AddTransient<UnfollowAccountCommandHandler>();
            services.AddTransient<UpdateWalletCommandHandler>();

            services.AddHostedService<KafkaConsumerBackgroundService>();

            eventRegister
                .Register<AccountCreatedEvent>()
                .Register<AvatarImageAccountChangedEvent>()
                .Register<GymCreatedEvent>()
                .Register<ProfileChangedEvent>()
                .Register<AccountFollowedEvent>()
                .Register<AddedAccountFollowerEvent>()
                .Register<RemovedAccountFollowerEvent>()
                .Register<AccountUnfollowedEvent>()
                .Register<SnapShotEvent<Account>>();

            services.AddMarten(options =>
                      {
                          options.Connection("host=localhost;database=ezgym-store;password=ezgym;username=postgres");
                          options.AutoCreateSchemaObjects = AutoCreate.All;
                          options.CreateDatabasesForTenants(c =>
                          {
                              c.ForTenant()
                                  .CheckAgainstPgDatabase()
                                  .WithOwner("postgres")
                                  .WithEncoding("UTF-8")
                                  .ConnectionLimit(-1)
                                  .OnDatabaseCreated(_ =>
                                  {
                                  });
                          });


                          options.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.NonPublicSetters);
                          options.Projections.SelfAggregate<User>();
                          options.Projections.SelfAggregate<Account>();
                      });


            return services;
        }
    }
}
