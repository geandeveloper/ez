using EzGym.Accounts;
using EzGym.Accounts.ChangeAvatar;
using EzGym.Accounts.CreateAccount;
using EzGym.Accounts.FollowAccount;
using EzGym.Accounts.UnfollowAccount;
using EzGym.Accounts.UpInsertAccountProfile;
using EzGym.Gyms;
using EzGym.Gyms.CreateGym;
using EzGym.Gyms.CreatePlan;
using EzGym.Gyms.Users;
using EzGym.Gyms.Users.CreateGymUser;
using EzGym.Gyms.Users.RegisterGymMemberShip;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;
using EzGym.Payments;
using EzGym.Payments.CreatePix;
using EzGym.Payments.Gateways;
using EzGym.Wallets;
using EzGym.Wallets.UpdateWallet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.Extensions.Configuration;
using StoreOptions = Marten.StoreOptions;

namespace EzGym
{
    public static class IoC
    {
        public static IServiceCollection AddEzGym(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IGymRepository, GymRepository>();

            services.AddSingleton<GatewayFactory>();

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

            services
                .AddMartenStore<IGymEventStore>(serviceProvider =>
                {
                    var settings = serviceProvider.GetService<IOptions<EzGymSettings>>()!;
                    var options = new StoreOptions();

                    options.Connection(settings.Value.Storage.Marten.ConnectionString);
                    options.CreateDatabasesForTenants(c =>
                    {
                        c.ForTenant()
                            .CheckAgainstPgDatabase()
                            .WithOwner("postgres")
                            .WithEncoding("UTF-8")
                            .ConnectionLimit(-1);
                    });


                    options.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.NonPublicSetters);
                    options.Events.StreamIdentity = StreamIdentity.AsString;

                    options.Projections.SelfAggregate<Account>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<Gym>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<GymUser>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<Payment>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<Wallet>(ProjectionLifecycle.Inline);

                    return options;
                })
                .ApplyAllDatabaseChangesOnStartup()
                .AddAsyncDaemon(DaemonMode.HotCold);

            return services;
        }
    }
}
