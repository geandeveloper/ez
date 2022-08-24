using System;
using EzCommon.Events;
using EzGym.Accounts;
using EzGym.Accounts.ChangeAvatar;
using EzGym.Accounts.CreateAccount;
using EzGym.Accounts.Events;
using EzGym.Accounts.FollowAccount;
using EzGym.Accounts.UnfollowAccount;
using EzGym.Accounts.UpInsertAccountProfile;
using EzGym.Gyms;
using EzGym.Gyms.CreateGym;
using EzGym.Gyms.CreatePlan;
using EzGym.Gyms.Events;
using EzGym.Gyms.Users.CreateGymUser;
using EzGym.Gyms.Users.RegisterGymMemberShip;
using EzGym.Infra.Storage;
using EzGym.Payments;
using EzGym.Payments.CreatePix;
using EzGym.Payments.Gateways;
using EzGym.Wallets;
using EzGym.Wallets.UpdateWallet;
using EzIdentity.Models;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace EzGym
{
    public static class IoC
    {
        public static IServiceCollection AddEzGym(this IServiceCollection services, IEventRegister eventRegister)
        {
            services.AddSingleton<GatewayFactory>();

            services.AddScoped<IGymEventStore, GymEventStore>();
            services.AddScoped<IGymQueryStore, GymEventStore>();

            services.AddTransient<CreateAccountCommandHandler>();
            services.AddTransient<CreateGymCommandHandler>();
            services.AddTransient<ChangeAvatarCommandHandler>();
            services.AddTransient<UpInsertAccountProfileCommandHandler>();
            services.AddTransient<FollowAccountCommandHandler>();
            services.AddTransient<UnfollowAccountCommandHandler>();
            services.AddTransient<UpdateWalletCommandHandler>();
            services.AddTransient<CreatePlanCommandHandler>();
            services.AddTransient<CreatePixCommandHandler>();
            services.AddTransient<CreateGymUserCommandHandler>();
            services.AddTransient<RegisterGymMemberShipCommandHandler>();

            //services.AddHostedService<KafkaConsumerBackgroundService>();

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
                                  .ConnectionLimit(-1)
                                  .OnDatabaseCreated(_ =>
                                  {

                                      Console.Write("worked");
                                  });
                          });


                          options.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.NonPublicSetters);

                          options.Events.StreamIdentity = StreamIdentity.AsString;
                          options.Projections.SelfAggregate<User>(ProjectionLifecycle.Inline);
                          options.Projections.SelfAggregate<Account>(ProjectionLifecycle.Inline);
                          options.Projections.SelfAggregate<Gym>(ProjectionLifecycle.Inline);
                          options.Projections.SelfAggregate<Payment>(ProjectionLifecycle.Inline);
                          options.Projections.SelfAggregate<Wallet>(ProjectionLifecycle.Inline);
                      });

            return services;
        }
    }
}
