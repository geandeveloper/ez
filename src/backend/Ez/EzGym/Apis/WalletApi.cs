using System.Threading;
using EzCommon.Models;
using EzGym.Events.Wallet;
using EzGym.Wallets.SetupPaymentAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzGym.Apis
{
    public static class WalletApi

    {
        public static WebApplication UseWalletApi(this WebApplication app)
        {
            app.MapPut("/wallets/{id}/setup-payment-account",
                [Authorize] async (
                      [FromServices] EzPrincipal principal,
                      [FromServices] SetupPaymentAccountCommandHandler handler,
                      string id,
                      SetupPaymentAccountCommand command) =>
                {
                    var eventStream = await handler.Handle(command, CancellationToken.None);
                    var @event = eventStream.GetEvent<PaymentAccountChangedEvent>();

                    return Results.Ok(@event);
                });

            return app;
        }

    }
}
