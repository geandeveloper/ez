using EzCommon.Models;
using EzGym.Gyms;
using EzGym.Gyms.Events;
using EzGym.Infra.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using EzGym.Gyms.CreatePlan;

namespace EzGym.Apis
{
    public static class GymApi
    {
        public static WebApplication UseEzGymGymsApi(this WebApplication app)
        {
            app.MapPost("/gyms/users", () =>
            {

            });

            app.MapPost("/gyms/{gymId}/plans",
                [Authorize] async (
                [FromServices] CreatePlanCommandHandler handler,
                CreatePlanCommand command,
                Guid gymId) =>
                {
                    var eventStream = await handler.Handle(command, CancellationToken.None);
                    var @event = eventStream.GetEvent<PlanCreatedEvent>();

                    return Results.Ok(@event);
                });

            app.MapGet("/gyms/{gymId}/plans",
                [Authorize] (
                [FromServices] EzPrincipal principal,
                [FromServices] IGymQueryStore query,
                Guid gymId) =>
                {
                    var plans = query
                    .Where<Gym>(gym => gym.Id == gymId)
                    .FirstOrDefault()?.Plans;

                    return Results.Ok(plans);
                });

            app.MapGet("/gyms/{id}/wallet", () =>
            {

            });

            return app;
        }
    }
}
