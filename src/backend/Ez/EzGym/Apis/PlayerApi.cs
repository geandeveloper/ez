using EzGym.Events.Player;
using EzGym.Players.CreateCheckIn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace EzGym.Apis
{
    public static class PlayerApi
    {
        public static WebApplication UsePlayerApi(this WebApplication app)
        {
            app.MapPost("/players/{id}/checkins",
                       [Authorize] async (
                         string id,
                         CreateCheckInCommand command,
                        [FromServices] CreateCheckInCommandHandler handler) =>
                       {
                           var eventStream = await handler.Handle(command, CancellationToken.None);
                           var @event  = eventStream.GetEvent<CheckInCreatedEvent>();
                           return Results.Ok(@event);
                       });

            return app;
        }
    }
}
