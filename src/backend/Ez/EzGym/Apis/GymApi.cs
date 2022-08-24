using EzCommon.Models;
using EzGym.Gyms;
using EzGym.Gyms.Events;
using EzGym.Infra.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using EzGym.Gyms.CreatePlan;
using EzGym.Gyms.Users.RegisterGymMemberShip;

namespace EzGym.Apis
{
    public static class GymApi
    {
        public static WebApplication UseEzGymGymsApi(this WebApplication app)
        {
            app.MapPost("/gyms/{gymId}/plans",
                [Authorize] async (
                [FromServices] CreatePlanCommandHandler handler,
                CreatePlanCommand command,
                string gymId) =>
                {
                    var eventStream = await handler.Handle(command, CancellationToken.None);
                    var @event = eventStream.GetEvent<PlanCreatedEvent>();

                    return Results.Ok(@event);
                });

            app.MapPost("/gyms/{gymId}/plans",
                [Authorize] async (
                [FromServices] CreatePlanCommandHandler handler,
                CreatePlanCommand command,
                string gymId) =>
                {
                    var eventStream = await handler.Handle(command, CancellationToken.None);
                    var @event = eventStream.GetEvent<PlanCreatedEvent>();

                    return Results.Ok(@event);
                });

            app.MapGet("/gyms/{gymId}/plans",
                [Authorize] (
                [FromServices] EzPrincipal principal,
                [FromServices] IGymQueryStore query,
                string gymId) =>
                {
                    var plans = query
                        .Where<Gym>(gym => gym.Id == gymId)
                        .First().GymPlans;

                    return Results.Ok(plans);
                });

            app.MapPost("/gyms/{gymId}/memberships",
                [Authorize] async (
                [FromServices] RegisterGymMemberShipCommandHandler handler,
                RegisterGymMemberShipCommand command,
                string gymId) =>
                {
                    var eventStream = await handler.Handle(command, CancellationToken.None);
                    var @event = eventStream.GetEvent<GymMemberShipRegisteredEvent>();

                    return Results.Ok(@event);
                });

            app.MapGet("/gyms/{gymId}/wallet", () =>
            {

            });

            return app;
        }
    }
}
