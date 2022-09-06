using System.Linq;
using System.Threading;
using EzCommon.Models;
using EzGym.Events.Wallet;
using EzGym.Infra.Repository;
using EzGym.Wallets;
using EzGym.Wallets.SetupPaymentAccount;
using EzPayment.Infra.Repository;
using EzPayment.Payments;
using Marten;
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

            app.MapGet("/wallets/{id}/receipts",
                [Authorize] async (
                      [FromServices] IPaymentRepository repository,
                      string id
                     ) =>
                {
                    var receipts = await repository.Where<WalletReceipt>(w => w.WalletId == id).ToListAsync();

                    return Results.Ok(receipts);
                });

            app.MapGet("/wallets/{id}/statement",
                [Authorize] async (
                      [FromServices] IGymRepository repository,
                      string id) =>
                {
                    var receipts = repository.Where<WalletReceipt>(w => w.WalletId == id).ToList();

                    var statement = new
                    {
                        TotalApproved = receipts.Where(r => r.PaymentStatus == PaymentStatusEnum.Approved).Sum(r => r.Amount),
                        TotalPending = receipts.Where(r => r.PaymentStatus == PaymentStatusEnum.Pending).Sum(r => r.Amount),
                        Balance = receipts.Sum(r => r.Amount),
                    };

                    return Results.Ok(statement);
                });

            return app;
        }

    }
}
