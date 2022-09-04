using EzPayment.Infra.Repository;
using EzPayment.Infra.Storage;
using EzPayment.Integrations.Gateways;
using EzPayment.Integrations.Gateways.StripePayments;
using EzPayment.PaymentAccounts;
using EzPayment.PaymentAccounts.CreatePaymentAccount;
using EzPayment.PaymentAccounts.CreatePaymentAccountSetupLink;
using EzPayment.PaymentAccounts.VerifyPaymentAccount;
using EzPayment.Payments;
using EzPayment.Payments.CreatePayment;
using EzPayment.Payments.PaymentReceived;
using EzPayment.Payments.VerifyCardPayments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using StoreOptions = Marten.StoreOptions;

namespace EzPayment
{
    public static class IoC
    {
        public static IServiceCollection AddEzPayment(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<PaymentGatewayFactory>();
            services.AddSingleton<IStripePaymentGateway>(new StripePaymentGateway(new SessionService(), new PaymentIntentService(), new AccountService(), new AccountLinkService()));

            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services
                .AddTransient<VerifyCardPaymentsCommandHandler>();

            services
                .AddTransient<VerifyPaymentAccountCommandHandler>();

            services
                .AddTransient<CreatePaymentCommandHandler>()
                .AddTransient<CreatePaymentService>();

            services
                .AddTransient<CreatePaymentAccountCommandHandler>()
                .AddTransient<CreatePaymentAccountService>();

            services
                .AddTransient<CreatePaymentAccountSetupLinkCommandHandler>()
                .AddTransient<CreatePaymentAccountSetupLinkService>();

            services.AddTransient<PaymentReceivedCommandHandler>();

            services
                .AddMartenStore<IPaymentStore>(serviceProvider =>
                {
                    var settings = serviceProvider.GetService<IOptions<EzPaymentSettings>>()!;
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

                    options.Projections.SelfAggregate<Payment>(ProjectionLifecycle.Async);
                    options.Projections.SelfAggregate<PaymentAccount>(ProjectionLifecycle.Async);

                    return options;
                })
                .ApplyAllDatabaseChangesOnStartup()
                .AddAsyncDaemon(DaemonMode.HotCold);

            return services;
        }
    }
}
