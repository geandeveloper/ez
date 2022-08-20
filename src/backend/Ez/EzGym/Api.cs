using EzCommon.Models;
using EzGym.Apis;
using EzGym.Infra.Storage;
using EzGym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EzGym
{
    public static class Api
    {
        public static WebApplication UseEzGymApi(this WebApplication app)
        {

            app.MapGet("/userinfo",
                [Authorize] (
                      [FromServices] EzPrincipal principal,
                      [FromServices] IGymQueryStore queryStorage) =>
                  {
                      var userInfo = new
                      {
                          principal.UserName,
                          Accounts = queryStorage.Where<Account>(account => account.UserId == principal.Id).ToList(),
                      };

                      return Results.Ok(userInfo);
                  });



            app.UseEzGymAccountApi()
                .UseEzGymGymsApi()
                .UsePaymentApi();

            return app;
        }

    }
}
