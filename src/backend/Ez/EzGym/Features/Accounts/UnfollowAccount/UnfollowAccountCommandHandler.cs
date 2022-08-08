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
        private readonly IGymQueryStore _queryStore;
        private readonly IGymEventStore _eventStore;

        public UnfollowAccountCommandHandler(IGymQueryStore queryStore, IGymEventStore eventStore)
        {
            _queryStore = queryStore;
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(UnfollowAccountCommand request, CancellationToken cancellationToken)
        {
            var snapShot = _queryStore.Query<AccountSnapShot>(a => a.Id == request.UserAccountId).First();
            var account = Account.RestoreSnapShot(snapShot);

            var accountToUnfollowSnapShot = _queryStore.Query<AccountSnapShot>(a => a.Id == request.UnfollowAccountId).First();
            var accountToUnfollow = Account.RestoreSnapShot(accountToUnfollowSnapShot);

            account.UnfollowAccount(accountToUnfollow);
            accountToUnfollow.RemoveFollower(account);

            await _eventStore.SaveAsync<Account, AccountSnapShot>(accountToUnfollow);
            return await _eventStore.SaveAsync<Account, AccountSnapShot>(account);
        }
    }
}
