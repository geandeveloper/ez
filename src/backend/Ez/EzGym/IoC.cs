using EzGym.Accounts;
using EzGym.Accounts.ChangeAvatar;
using EzGym.Accounts.CreateAccount;
using EzGym.Accounts.Followers;
using EzGym.Accounts.UpInsertAccountProfile;
using EzGym.Gyms;
using EzGym.Gyms.CreateGym;
using EzGym.Gyms.CreatePlan;
using EzGym.Gyms.RegisterGymMemberShip;
using EzGym.Gyms.Users;
using EzGym.Gyms.Users.CreateGymUser;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;
using EzGym.Projections;
using EzGym.Wallets;
using EzGym.Wallets.SetupPaymentAccount;
using EzGym.Wallets.UpdateWallet;
using EzPayment.Integrations.Gateways;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.Extensions.Configuration;
using StoreOptions = Marten.StoreOptions;
using EzGym.Accounts.Followers.UnfollowAccount;
using EzGym.Accounts.Followers.FollowAccount;
using EzGym.Players;
using EzGym.Players.CreatePlayer;

namespace EzGym
{
    public static class IoC
    {
        public static IServiceCollection AddEzGym(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IGymRepository, GymRepository>();
            services.AddSingleton<PaymentGatewayFactory>();

            services.AddTransient<CreateAccountCommandHandler>();
            services.AddTransient<CreateGymCommandHandler>();
            services.AddTransient<ChangeAvatarCommandHandler>();
            services.AddTransient<UpInsertAccountProfileCommandHandler>();
            services.AddTransient<FollowAccountCommandHandler>();
            services.AddTransient<UnfollowAccountCommandHandler>();
            services.AddTransient<UpdateWalletCommandHandler>();
            services.AddTransient<CreatePlanCommandHandler>();
            services.AddTransient<CreateGymUserCommandHandler>();
            services.AddTransient<RegisterGymMemberShipCommandHandler>();
            services.AddTransient<SetupPaymentAccountCommandHandler>();
            services.AddTransient<CreatePlayerCommandHandler>();

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
                    options.Schema.For<Account>().UniqueIndex(a => a.AccountName);

                    options.Projections.SelfAggregate<Follower>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<Gym>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<GymUser>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<Wallet>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<WalletReceipt>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<GymMemberShip>(ProjectionLifecycle.Inline);
                    options.Projections.SelfAggregate<Player>(ProjectionLifecycle.Inline);


                    //Custom Projections
                    options.Projections.Add<AccountProfileProjection>(ProjectionLifecycle.Inline);
                    options.Projections.Add<SearchAccountsProjection>(ProjectionLifecycle.Async);
                    options.Projections.Add<AccountFollowersProjection>(ProjectionLifecycle.Async);
                    options.Projections.Add<AccountFollowingsProjection>(ProjectionLifecycle.Async);
                    options.Projections.Add<AccountMemberShipProjection>(ProjectionLifecycle.Async);

                    return options;
                })
                .ApplyAllDatabaseChangesOnStartup()
                .AddAsyncDaemon(DaemonMode.HotCold);

            return services;
        }
    }
}
