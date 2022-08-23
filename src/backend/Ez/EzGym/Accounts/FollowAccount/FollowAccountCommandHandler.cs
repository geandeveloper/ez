using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;

namespace EzGym.Accounts.FollowAccount
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
            var account = await _eventStore.LoadAggregateAsync<Account>(request.UserAccountId);
            var accountToFollow = await _eventStore.LoadAggregateAsync<Account>(request.FollowAccountId);

            account.FollowAccount(accountToFollow);
            accountToFollow.AddFollower(account);

            await _eventStore.SaveAggregateAsync(accountToFollow);
            return await _eventStore.SaveAggregateAsync(account);
        }
    }
}
