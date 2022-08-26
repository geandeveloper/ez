using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;

namespace EzGym.Accounts.FollowAccount
{
    public class FollowAccountCommandHandler : ICommandHandler<FollowAccountCommand>
    {
        private readonly IGymRepository _repository;

        public FollowAccountCommandHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventStream> Handle(FollowAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.LoadAggregateAsync<Account>(request.UserAccountId);
            var accountToFollow = await _repository.LoadAggregateAsync<Account>(request.FollowAccountId);

            account.FollowAccount(accountToFollow);
            accountToFollow.AddFollower(account);

            await _repository.SaveAggregateAsync(accountToFollow);
            return await _repository.SaveAggregateAsync(account);
        }
    }
}
