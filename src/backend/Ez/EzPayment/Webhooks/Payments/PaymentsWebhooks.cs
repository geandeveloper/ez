using System;
using System.IO;
using System.Threading;
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

            var handler = context.RequestServices.GetRequiredService<PaymentReceivedCommandHandler>();
            var command = new PaymentReceivedCommand(paymentIntent!.Id, paymentIntent.Amount);

            await handler.Handle(command, CancellationToken.None);
        });

        return app;
    }

}
