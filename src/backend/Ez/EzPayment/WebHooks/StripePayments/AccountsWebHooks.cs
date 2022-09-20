using System.IO;
using System.Threading;
using EzPayment.Infra.Repository;
using EzPayment.PaymentAccounts;
using EzPayment.PaymentAccounts.VerifyPaymentAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;

namespace EzPayment.WebHooks.StripePayments
{
    public static class AccountsWebHooks
    {
        public static WebApplication UseAccountsWebHooks(this WebApplication app)
        {
            app.MapPost("/webhooks/stripe/accounts", async (context) =>
            {
                var settings = context.RequestServices.GetService<IOptions<EzPaymentSettings>>();
                var json = await new StreamReader(context.Request.Body).ReadToEndAsync();

                var stripeEvent = EventUtility.ConstructEvent(json,
                    context.Request.Headers["Stripe-Signature"], settings!.Value.StripePayments.WebhookSecret, 300, false);

                if (stripeEvent.Type != Stripe.Events.AccountUpdated)
                    return;

                var repository = context.RequestServices.GetRequiredService<IPaymentRepository>();
                var paymentAccount = await repository.QueryAsync<PaymentAccount>(p => p.IntegrationInfo.Id == stripeEvent.Account);


                if (paymentAccount == null)
                    return;

                var handler = context.RequestServices.GetRequiredService<VerifyPaymentAccountCommandHandler>();
                await handler.Handle(new VerifyPaymentAccountCommand(paymentAccount.Id), CancellationToken.None);
            });

            return app;
        }

    }
}
