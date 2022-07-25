using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.CreateGym;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace EzGym
{
    public static class Api
    {
        public static WebApplication UseEzGymApi(this WebApplication app)
        {
            app.MapPost("/ezgym/gyms",
                [Authorize]
            async (
                      [FromServices] EzPrincipal principal,
                      [FromServices] CreateGymCommandHandler handler,
                      CreateGymCommand command) =>
                  {
                      var eventStream = await handler.Handle(command with { OwnerId = principal.Id.Value }, CancellationToken.None);
                      return Results.Ok(eventStream.GetEvent<GymCreatedEvent>());
                  });

            return app;

        }

    }
}
