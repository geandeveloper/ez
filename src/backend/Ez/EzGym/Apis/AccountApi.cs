using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Accounts.Events;
using EzGym.Gyms;
using EzGym.Infra.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using EzGym.Accounts.ChangeAvatar;
using EzGym.Accounts.CreateAccount;
using EzGym.Accounts.FollowAccount;
using EzGym.Accounts.UnfollowAccount;
using EzGym.Accounts.UpInsertAccountProfile;
using EzGym.Gyms.CreateGym;
using EzGym.Infra.Repository;
using EzGym.Wallets;
using EzGym.Wallets.UpdateWallet;
using EzGym.Events.Gym;

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
                      string accountId,
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
                      [FromServices] IGymRepository repository, string accountName) =>
                  {
                      var response = new
                      {
                          Exists = repository.Where<Account>(account => account.AccountName == accountName).FirstOrDefault() != null
                      };

                      return Results.Ok(response);
                  });

            app.MapGet("accounts",
                        [Authorize] (
                              [FromServices] IGymRepository repository,
                              string query
                              ) =>
                        {
                            var accounts = repository.Where<Account>(a => a.AccountName.Contains(query))
                            .Take(20)
                            .ToList();

                            return Results.Ok(accounts);
                        });

            app.MapGet("accounts/{accountName}",
            [Authorize]
            (
                      [FromServices] IGymRepository repository,
                      string accountName
                      ) =>
                {
                    var account = repository.Where<Account>(a => a.AccountName == accountName).FirstOrDefault();

                    return Results.Ok(account);
                });

            app.MapGet("accounts/{accountName}/followers",
                [Authorize] (
                     [FromServices] IGymRepository repository,
                     string accountName,
                     string query
                     ) =>
               {
                   //var followers = queryStorage.QueryFollowers(accountName, query);

                   return Results.Ok(null);
               });

            app.MapGet("accounts/{accountName}/following",
                [Authorize] (
                      [FromServices] IGymRepository repository,
                      string accountName, 
                      string query
                      ) =>
                {
                    return Results.Ok("");
                });

            app.MapPut("accounts/{accountId}/profile",
                [Authorize] async (
                      [FromServices] UpInsertAccountProfileCommandHandler handler,
                      UpInsertAccountProfileCommand command,
                      string accountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<ProfileChangedEvent>());
                });

            app.MapPost("accounts/{followAccountId}/follow",
                [Authorize] async (
                      [FromServices] FollowAccountCommandHandler handler,
                      FollowAccountCommand command,
                      string followAccountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command with { FollowAccountId = followAccountId }, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<AccountFollowedEvent>());
                });

            app.MapPost("accounts/{unfollowAccountId}/unfollow",
                [Authorize] async (
                      [FromServices] UnfollowAccountCommandHandler handler,
                      UnfollowAccountCommand command,
                      string unfollowAccountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command with { UnfollowAccountId = unfollowAccountId }, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<AccountUnfollowedEvent>());
                });

            app.MapPost("accounts/{accountId}/wallet",
                [Authorize] async (
                      [FromServices] UpdateWalletCommandHandler handler,
                      UpdateWalletCommand command,
                      string accountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command with { AccountId = accountId }, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<AccountWalletChangedEvent>());
                });

            app.MapGet("accounts/{accountId}/wallet",
                [Authorize] (
                      [FromServices] IGymRepository repository,
                      string accountId
                      ) =>
                {
                    var wallet = repository.QueryOne<Wallet>(a => a.AccountId == accountId);

                    return Results.Ok(wallet);
                });

            app.MapGet("accounts/{accountId}/gym",
                [Authorize] (
                      [FromServices] IGymRepository repository,
                      string accountId
                      ) =>
                {
                    var gym = repository.QueryOne<Gym>(a => a.AccountId == accountId);

                    return Results.Ok(gym);
                });


            return app;
        }
    }
}
