using EzCommon.Models;
using EzGym.Apis;
using EzGym.Infra.Storage;
using EzGym.SnapShots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                          Accounts = queryStorage.Query<AccountSnapShot>(account => account.UserId == principal.Id)
                      };

                      return Results.Ok(userInfo);
                  });



            app.UseEzGymAccountApi();

            return app;
        }

    }
}
