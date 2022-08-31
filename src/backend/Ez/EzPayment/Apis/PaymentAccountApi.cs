using System.Threading;
using EzPayment.Events.Accounts;
using EzPayment.PaymentAccounts.VerifyPaymentAccount;
using EzPayment.WebHooks.StripePayments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzPayment.Apis;

public static class PaymentAccountApi
{
    public static WebApplication UseAccountPaymentApi(this WebApplication app)
    {
        app.UsePaymentWebHooks().UsePaymentWebHooks().UseAccountsWebHooks();

        app.MapPost("/payment-accounts/{id}/verify",
            [Authorize] async (
                [FromServices] VerifyPaymentAccountCommandHandler handler,
                VerifyPaymentAccountCommand command, string id) =>
        {
            var eventStream = await handler.Handle(command, CancellationToken.None);
            var @event = eventStream.GetEvent<PaymentAccountStatusChanged>();

            return Results.Ok(@event);
        });

        return app;
    }
}