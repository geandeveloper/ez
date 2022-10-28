using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Gyms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading;
using EzGym.Accounts.ChangeAvatar;
using EzGym.Accounts.CreateAccount;
using EzGym.Accounts.UpInsertAccountProfile;
using EzGym.Gyms.CreateGym;
using EzGym.Infra.Repository;
using EzGym.Wallets;
using EzGym.Wallets.UpdateWallet;
using EzGym.Events.Gym;
using EzGym.Projections;
using EzPayment.Payments.VerifyCardPayments;
using Marten;
using EzGym.Accounts.Followers.FollowAccount;
using EzGym.Accounts.Followers.UnfollowAccount;
using EzGym.Players;
using EzGym.Events.Accounts;

namespace EzGym.Apis
{
    public static class AccountApi
    {
        public static WebApplication UseAccountApi(this WebApplication app)
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
                [Authorize]
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
                            var accounts = repository
                                .Where<SearchAccounts>(a =>
                                    a.AccountName.NgramSearch(query) ||
                                    a.ProfileName.NgramSearch(query))
                            .Take(20)
                            .ToList();

                            return Results.Ok(accounts);
                        });

            app.MapGet("accounts/{accountName}/profile",
            [Authorize]
            (
                      [FromServices] EzPrincipal principal,
                      [FromServices] IGymRepository repository,
                      string accountName
                      ) =>
                {
                    var accountProfile = repository.Where<AccountProfile>(a => a.AccountName == accountName).FirstOrDefault();

                    return Results.Ok(accountProfile);
                });

            app.MapGet("accounts/{id}/followers",
                [Authorize]
            (
                     [FromServices] IGymRepository repository,
                     string id,
                     string query
                     ) =>
            {
                var queryable = repository
                    .Where<AccountFollower>(a => a.AccountId == id);

                if (!string.IsNullOrEmpty(query))
                    queryable = queryable.Where(a => a.ProfileName.NgramSearch(query));

                return Results.Ok(queryable.Take(20).ToList());
            });

            app.MapGet("accounts/{id}/followers/count",
                [Authorize] async (
                     [FromServices] IGymRepository repository,
                     string id
                     ) =>
                {
                    var total = await repository
                        .Where<AccountFollower>(a => a.AccountId == id).CountAsync();

                    return Results.Ok(new
                    {
                        Total = total
                    });
                });

            app.MapGet("accounts/{id}/followers/{followerAccountId}",
                [Authorize] async (
                     [FromServices] IGymRepository repository,
                     string id,
                     string followerAccountId
                     ) =>
                {
                    var isFollowing = await repository
                        .Where<AccountFollower>(a => a.AccountId == id)
                        .Where(f => f.FollowerAccountId == followerAccountId)
                        .AnyAsync();

                    return Results.Ok(new
                    {
                        IsFollowing = isFollowing
                    });
                });

            app.MapGet("accounts/{id}/following",
                [Authorize]
            (
                      [FromServices] IGymRepository repository,
                      string id,
                      string query
                      ) =>
                {
                    var queryable = repository
                     .Where<AccountFollowing>(a => a.AccountId == id);

                    if (!string.IsNullOrEmpty(query))
                        queryable = queryable.Where(a => a.ProfileName.NgramSearch(query));

                    return Results.Ok(queryable.Take(20).ToList());
                });

            app.MapGet("accounts/{id}/following/count",
                [Authorize] async (
                     [FromServices] IGymRepository repository,
                     string id
                     ) =>
                {
                    var total = await repository
                        .Where<AccountFollowing>(a => a.AccountId == id).CountAsync();

                    return Results.Ok(new
                    {
                        Total = total
                    });
                });

            app.MapPut("accounts/{id}/profile",
                [Authorize] async (
                      [FromServices] UpInsertAccountProfileCommandHandler handler,
                      UpInsertAccountProfileCommand command,
                      string id
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

                    return Results.Ok(eventStream.GetEvent<FollowerCreatedEvent>());
                });

            app.MapPost("accounts/{unfollowAccountId}/unfollow",
                [Authorize] async (
                      [FromServices] UnfollowAccountCommandHandler handler,
                      UnfollowAccountCommand command,
                      string unfollowAccountId
                      ) =>
                {
                    var eventStream = await handler.Handle(command with { UnfollowAccountId = unfollowAccountId }, CancellationToken.None);

                    return Results.Ok(eventStream.GetEvent<FollowDeleteEvent>());
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
                    [FromServices] VerifyCardPaymentsCommandHandler handler,
                      string accountId
                      ) =>
                {

                    repository
                        .Where<GymMemberShip>(m => m.ReceiverAccountId == accountId)
                        .Where(m => !m.Active)
                        .ToList()
                        .ForEach(m =>
                        {
                            handler.Handle(new VerifyCardPaymentsCommand(m.PaymentId), CancellationToken.None).Wait();
                        });


                    var wallet = repository.QueryOne<Wallet>(a => a.AccountId == accountId);


                    return System.Threading.Tasks.Task.FromResult(Results.Ok(wallet));
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

            app.MapGet("/accounts/{id}/player",
                [Authorize] (
                        [FromServices] IGymRepository repository,
                        string id
                        ) =>
                {
                    var player = repository.QueryOne<Player>(p => p.AccountId == id);
                    return Results.Ok(player);
                });


            app.MapGet("/accounts/{accountId}/memberships",
                [Authorize] async (
                [FromServices] IGymRepository repository,
                string accountId) =>
                {
                    var accountMemberShips = await repository.Where<AccountMemberShip>(a=> a.AccountId == accountId).ToListAsync();

                    return Results.Ok(accountMemberShips);
                });

            return app;
        }
    }
}
