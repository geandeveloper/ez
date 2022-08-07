using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using EzGym.SnapShots;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Accounts.FollowAccount
{
    public class FollowAccountCommandHandler : ICommandHandler<FollowAccountCommand>
    {
        private readonly IGymQueryStore _queryStore;
        private readonly IGymEventStore _eventStore;

        public FollowAccountCommandHandler(IGymQueryStore queryStore, IGymEventStore eventStore)
        {
            _queryStore = queryStore;
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(FollowAccountCommand request, CancellationToken cancellationToken)
        {
            var snapShot = _queryStore.Query<AccountSnapShot>(a => a.Id == request.UserAccountId).First();
            var account = Account.RestoreSnapShot(snapShot);

            var accountToFollowSnapShot = _queryStore.Query<AccountSnapShot>(a => a.Id == request.FollowAccountId).First();
            var accountToFollow = Account.RestoreSnapShot(accountToFollowSnapShot);

            account.FollowAccount(accountToFollow);
            accountToFollow.AddFollower(account);

            await _eventStore.SaveAsync<Account, AccountSnapShot>(accountToFollow);
            return await _eventStore.SaveAsync<Account, AccountSnapShot>(account);
        }
    }
}
