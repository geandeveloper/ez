using System.IO;
using System.Threading;
using EzPayment.Infra.Repository;
using EzPayment.Payments;
using EzPayment.Payments.PaymentReceived;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;

namespace EzPayment.Webhooks.Payments;

public static class PaymentsWebhooks
{
    public static WebApplication UsePaymentWebHooks(this WebApplication app)
    {


        app.MapPost("/webhooks/stripe/payments", async (
            context

            ) =>
        {
            var settings = context.RequestServices.GetService<IOptions<EzPaymentSettings>>();
            var json = await new StreamReader(context.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                context.Request.Headers["Stripe-Signature"], settings!.Value.StripePayments.WebhookSecret, 300, false);

            if (stripeEvent.Type != Stripe.Events.PaymentIntentSucceeded)
                return;

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            var repository = context.RequestServices.GetRequiredService<IPaymentRepository>();
            var handler = context.RequestServices.GetRequiredService<PaymentReceivedCommandHandler>();


            var payment = repository.QueryOne<Payment>(p => p.CardInfo.IntegrationId == paymentIntent.Id);
            var command = new PaymentReceivedCommand(payment!.Id, payment.Amount);
            await handler.Handle(command, CancellationToken.None);
        });

        return app;
    }

}
