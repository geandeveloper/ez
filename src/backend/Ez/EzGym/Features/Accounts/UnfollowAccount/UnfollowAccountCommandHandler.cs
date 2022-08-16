using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Accounts.UnfollowAccount
{
    public class UnfollowAccountCommandHandler : ICommandHandler<UnfollowAccountCommand>
    {
        private readonly IGymEventStore _eventStore;

        public UnfollowAccountCommandHandler(IGymEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(UnfollowAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _eventStore.LoadAggregateAsync<Account>(request.UserAccountId);
            var accountToUnfollow = await _eventStore.LoadAggregateAsync<Account>(request.UnfollowAccountId);

            account.UnfollowAccount(accountToUnfollow);
            accountToUnfollow.RemoveFollower(account);

            await _eventStore.SaveAggregateAsync(accountToUnfollow);
            return await _eventStore.SaveAggregateAsync(account);
        }
    }
}
