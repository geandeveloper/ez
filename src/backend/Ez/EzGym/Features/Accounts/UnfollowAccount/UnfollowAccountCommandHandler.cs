using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using EzGym.SnapShots;
using System.Linq;
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
            var account = await _eventStore.QueryLatestVersionAsync<AccountSnapShot, Account>(a => a.Id == request.UserAccountId);
            var accountToUnfollow = await _eventStore.QueryLatestVersionAsync<AccountSnapShot, Account>(a => a.Id == request.UnfollowAccountId);

            account.UnfollowAccount(accountToUnfollow);
            accountToUnfollow.RemoveFollower(account);

            await _eventStore.SaveAsync<Account, AccountSnapShot>(accountToUnfollow);
            return await _eventStore.SaveAsync<Account, AccountSnapShot>(account);
        }
    }
}
