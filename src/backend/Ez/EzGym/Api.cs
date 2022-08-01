using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Accounts.ChangeAvatar;
using EzGym.Features.Accounts.CreateAccount;
using EzGym.Features.Gyms.CreateGym;
using EzGym.Infra.Storage;
using EzGym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace EzGym
{
    public static class Api
    {
        public static WebApplication UseEzGymApi(this WebApplication app)
        {
            app.MapPost("/accounts",
                [Authorize]
            async (
                      [FromServices] CreateAccountCommandHandler handler,
                      [FromServices] EzPrincipal principal,
                      CreateAccountCommand command) =>
                  {
                      var eventStream = await handler.Handle(command with { UserId = principal.Id }, CancellationToken.None);
                      return Results.Ok(eventStream.GetEvent<AccountCreatedEvent>());
                  });

            app.MapPost("/accounts/{accountId}/gyms",
                [Authorize]
            async (
                      [FromServices] CreateGymCommandHandler handler,
                      CreateGymCommand command) =>
                  {
                      var eventStream = await handler.Handle(command, CancellationToken.None);
                      return Results.Ok(eventStream.GetEvent<GymCreatedEvent>());
                  });

            app.MapPost("/accounts/{accountId}/avatar",
            async (
                      [FromServices] ChangeAvatarCommandHandler handler,
                      [FromServices] EzPrincipal principal,
                      Guid accountId,
                      HttpRequest request) =>
                  {
                      using (var avatarMemoryStream = new MemoryStream())
                      {

                          var form = await request.ReadFormAsync();
                          var file = form.Files["avatar"];

                          await using var fileStream = file.OpenReadStream();
                          await fileStream.CopyToAsync(avatarMemoryStream);

                          var eventStream = await handler.Handle(new ChangeAvatarCommand(
                              UserId: principal.Id,
                              AccountId: accountId,
                              FileName: file.FileName,
                              AvatarStream: avatarMemoryStream), CancellationToken.None);

                          return Results.Ok(eventStream.GetEvent<AvatarImageAccountChanged>());

                      }

                  })
                .Accepts<IFormFile>("multipart/form-data");

            app.MapPost("/accounts/{accountId}/gyms/{gymId}/users",
                    [Authorize]
            async (
                          [FromServices] CreateGymCommandHandler handler,
                          CreateGymCommand command) =>
                      {
                          var eventStream = await handler.Handle(command, CancellationToken.None);
                          return Results.Ok(eventStream.GetEvent<GymCreatedEvent>());
                      });

            app.MapGet("/accounts/{accountName}/verify", (
                      [FromServices] IGymQueryStorage queryStorage, string accountName) =>
                  {
                      var response = new
                      {
                          Exists = queryStorage.Query<Account>(account => account.AccountName == accountName).FirstOrDefault() != null
                      };

                      return Results.Ok(response);
                  });

            app.MapGet("/userinfo",
                [Authorize] (
                      [FromServices] EzPrincipal principal,
                      [FromServices] IGymQueryStorage queryStorage) =>
                  {
                      var userInfo = new
                      {
                          principal.UserName,
                          Accounts = queryStorage.Query<Account>(account => account.UserId == principal.Id)
                      };

                      return Results.Ok(userInfo);
                  });

            return app;
        }

    }
}
