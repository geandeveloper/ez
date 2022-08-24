using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using EzGym.Payments.CreatePix;
using EzGym.Payments.Events;

namespace EzGym.Apis;

public static class PaymentApi
{
    public static WebApplication UsePaymentApi(this WebApplication app)
    {
        app.MapPost("/payments",
             async (
                  [FromServices] CreatePixCommandHandler handler,
                   CreatePaymentCommand command) =>
              {

                  var eventStream = await handler.Handle(command, CancellationToken.None);
                  var @event = eventStream.GetEvent<PaymentCreatedEvent>();

                  return Results.Ok(@event);
              });

        return app;
    }
}

