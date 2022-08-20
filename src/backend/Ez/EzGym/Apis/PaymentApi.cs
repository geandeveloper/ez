using EzGym.Features.Payments;
using EzGym.Features.Payments.Pix;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace EzGym.Apis;

public static class PaymentApi
{
    public static WebApplication UsePaymentApi(this WebApplication app)
    {
        app.MapPost("/payments/pix",
             async (
                  [FromServices] CreatePixCommandHandler hanlder,
                  [FromServices] CreatePixCommand command) =>
              {

                  var eventStream = await hanlder.Handle(command, CancellationToken.None);
                  var @event = eventStream.GetEvent<PixCreatedEvent>();

                  return Results.Ok(@event);
              });


        return app;
    }
}

