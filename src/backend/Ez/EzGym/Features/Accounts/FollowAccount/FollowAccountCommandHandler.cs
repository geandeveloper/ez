using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using EzGym.SnapShots;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Accounts.FollowAccount
{
    public class FollowAccountCommandHandler : ICommandHandler<FollowAccountCommand>
    {
        private readonly IGymEventStore _eventStore;

        public FollowAccountCommandHandler(IGymEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(FollowAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _eventStore.QueryLatestVersionAsync<AccountSnapShot, Account>(a => a.Id == request.UserAccountId);
            var accountToFollow = await _eventStore.QueryLatestVersionAsync<AccountSnapShot, Account>(a => a.Id == request.FollowAccountId);

            account.FollowAccount(accountToFollow);
            accountToFollow.AddFollower(account);

            await _eventStore.SaveAsync<Account, AccountSnapShot>(accountToFollow);
            return await _eventStore.SaveAsync<Account, AccountSnapShot>(account);
        }
    }
}
