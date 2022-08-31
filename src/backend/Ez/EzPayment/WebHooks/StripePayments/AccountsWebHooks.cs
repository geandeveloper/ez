using System.IO;
using System.Threading;
using EzPayment.Infra.Repository;
using EzPayment.Payments;
using EzPayment.Payments.PaymentReceived;
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
                var handler = context.RequestServices.GetRequiredService<PaymentReceivedCommandHandler>();

            });

            return app;
        }

    }
}
