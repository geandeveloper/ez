using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using EzGym.Infra.Repository;

namespace EzGym
{
    public static class Api
    {
        public static WebApplication UseEzGymApi(this WebApplication app)
        {

            app.MapGet("/userinfo",
                [Authorize] (
                      [FromServices] EzPrincipal principal,
                      [FromServices] IGymRepository repository) =>
                  {
                      var userInfo = new
                      {
                          principal.UserName,
                          Accounts = repository.Where<Account>(account => account.UserId == principal.Id).ToList(),
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
