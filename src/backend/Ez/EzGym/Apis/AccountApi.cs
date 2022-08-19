using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Accounts.ChangeAvatar;
using EzGym.Features.Accounts.CreateAccount;
using EzGym.Features.Accounts.FollowAccount;
using EzGym.Features.Accounts.UnfollowAccount;
using EzGym.Features.Accounts.UpdateWallet;
using EzGym.Features.Accounts.UpInsertAccountProfile;
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

namespace EzGym.Apis
{
    public static class AccountApi
    {
        public static WebApplication UseEzGymAccountApi(this WebApplication app)
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

                          return Results.Ok(eventStream.GetEvent<AvatarImageAccountChangedEvent>());

                      }

                  }).Accepts<IFormFile>("multipart/form-data");

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
                      [FromServices] IGymQueryStore queryStorage, string accountName) =>
                  {
                      var response = new
                      {
                          Exists = queryStorage.Where<Account>(account => account.AccountName == accountName).FirstOrDefault() != null
                      };

                      return Results.Ok(response);
                  });

            app.MapGet("accounts",
                        [Authorize] (
                              [FromServices] IGymQueryStore queryStorage,
                              string query
                              ) =>
                        {
                            var accounts = queryStorage.Where<Account>(a => a.AccountName.Contains(query))
                            .Take(20)
                            .ToList();

                            return Results.Ok(accounts);
                        });

            app.MapGet("accounts/{accountName}",
            [Authorize]
            (
                      [FromServices] IGymQueryStore queryStorage,
                      string accountName
                      ) =>
                {
                    var account = queryStorage.Where<Account>(a => a.AccountName == accountName).FirstOrDefault();

                    return Results.Ok(account);
                });

            app.MapGet("accounts/{accountName}/followers",
                [Authorize] (
                     [FromServices] IGymQueryStore queryStorage,
                     string accountName,
                     string query
                     ) =>
               {
                   var followers = queryStorage.QueryFollowers(accountName, query);

                   return Results.Ok(followers);
               });

            app.MapGet("accounts/{accountName}/following",
                [Authorize] (
                      [FromServices] IGymQueryStore queryStorage,
                      string accountName,
                      string query
                      ) =>
                {
                    var following = queryStorage.QueryFollowing(accountName, query);

                    return Results.Ok(following);
                });

            app.MapPut("accounts/{accountId}/profile",
                [Authorize] async (
                      [FromServices] UpInsertAccountProfileCommandHandler handler,
                      UpInsertAccountProfileCommand command,
                      Guid accountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<ProfileChangedEvent>());
                });

            app.MapPost("accounts/{followAccountId}/follow",
                [Authorize] async (
                      [FromServices] FollowAccountCommandHandler handler,
                      FollowAccountCommand command,
                      Guid followAccountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command with { FollowAccountId = followAccountId }, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<AccountFollowedEvent>());
                });

            app.MapPost("accounts/{unfollowAccountId}/unfollow",
                [Authorize] async (
                      [FromServices] UnfollowAccountCommandHandler handler,
                      UnfollowAccountCommand command,
                      Guid unfollowAccountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command with { UnfollowAccountId = unfollowAccountId }, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<AccountUnfollowedEvent>());
                });

            app.MapPost("accounts/{accountId}/wallet",
                [Authorize] async (
                      [FromServices] UpdateWalletCommandHandler handler,
                      UpdateWalletCommand command,
                      Guid accountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command with { AccountId = accountId }, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<AccountWalletChangedEvent>());
                });

            app.MapGet("accounts/{accountId}/wallet",
                [Authorize] (
                      [FromServices] IGymQueryStore queryStore,
                      Guid accountId
                      ) =>
                {
                    var wallet = queryStore.QueryOne<Account>(a => a.Id == accountId).Wallet;

                    return Results.Ok(wallet);
                });

            app.MapGet("accounts/{accountId}/gym",
                [Authorize] (
                      [FromServices] IGymQueryStore queryStore,
                      Guid accountId
                      ) =>
                {
                    var gym = queryStore.QueryOne<Gym>(a => a.AccountId == accountId);

                    return Results.Ok(gym);
                });


            return app;
        }
    }
}
