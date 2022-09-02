using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using EzCommon.Infra.Storage;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;
using EzGym.Projections;
using Microsoft.Extensions.DependencyInjection;

namespace EzGym
{
    public static class Api
    {
        public static WebApplication UseEzGymApi(this WebApplication app)
        {

            app.MapGet("/my/accounts",
                       [Authorize] (
                             [FromServices] EzPrincipal principal,
                             [FromServices] IGymRepository repository) =>
                       {
                           var accounts = repository.Where<Account>(account => account.UserId == principal.Id).ToList();

                           return Results.Ok(accounts);
                       });


            app.UseAccountApi()
                .UseGymApi()
                .UseWalletApi();

            //app.Use((context, next) =>
            //{

            //    var eventStore = context.RequestServices.GetService<IGymEventStore>();

            //    eventStore.UseDaemonProjectionAsync(daemon =>
            //    {
            //        daemon.RebuildProjection<AccountFollowersProjection>(CancellationToken.None).Wait();
            //        daemon.RebuildProjection<AccountFollowingProjection>(CancellationToken.None).Wait();

            //    }).Wait();


            //    return next(context);
            //});

            return app;
        }

    }
}
