using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
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
            var userAccount = _queryStore.Query<Account>(a => a.Id == request.UserAccountId).First();
            var accountToFollow = _queryStore.Query<Account>(a => a.Id == request.FollowAccountId).First();

            userAccount.FollowAccount(accountToFollow);
            accountToFollow.AddFollower(userAccount);

            await _eventStore.SaveAsync(accountToFollow);

            return await _eventStore.SaveAsync(userAccount);
        }
    }
}
