using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;

namespace EzGym.Accounts.UnfollowAccount
{
    public class UnfollowAccountCommandHandler : ICommandHandler<UnfollowAccountCommand>
    {
        private readonly IGymRepository _repository;

        public UnfollowAccountCommandHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventStream> Handle(UnfollowAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.LoadAggregateAsync<Account>(request.UserAccountId);
            var accountToUnfollow = await _repository.LoadAggregateAsync<Account>(request.UnfollowAccountId);

            account.UnfollowAccount(accountToUnfollow);
            accountToUnfollow.RemoveFollower(account);

            await _repository.SaveAggregateAsync(accountToUnfollow);
            return await _repository.SaveAggregateAsync(account);
        }
    }
}
