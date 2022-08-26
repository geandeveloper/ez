using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using EzIdentity.Extensions;
using EzIdentity.Infra.Storage;
using EzIdentity.Infra.Repository;
using Marten.Events.Daemon.Resiliency;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using EzIdentity.Users;
using EzIdentity.Users.CreateUser;
using EzIdentity.Users.Login;
using EzIdentity.Users.RevokeToken;
using EzIdentity.Users.UpdateRefreshToken;

namespace EzIdentity;

public static class IoC
{
    public static IServiceCollection AddEzIdentity(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IIdentityRepository, IdentityRepository>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddTransient<LoginCommandHandler>();
        services.AddTransient<CreateUserCommandHandler>();
        services.AddTransient<UpdateRefreshTokenCommandHandler>();
        services.AddTransient<RevokeTokenCommandHandler>();

        services.AddAuthWithJwtBearer(configuration);

        services.AddMartenStore<IIdentityEventStore>(serviceProvider =>
            {
                var settings = serviceProvider.GetService<IOptions<EzIdentitySettings>>()!;
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

                options.Projections.SelfAggregate<User>(ProjectionLifecycle.Inline);

                return options;
            })
            .ApplyAllDatabaseChangesOnStartup()
            .AddAsyncDaemon(DaemonMode.HotCold);

        return services;
    }
}

