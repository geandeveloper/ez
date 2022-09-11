using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;
using Marten;

namespace EzGym.Accounts.Followers.UnfollowAccount
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
            var follower = await _repository.Where<Follower>(f => f.AccountId == request.UnfollowAccountId)
                .Where(f => f.FollowerAccountId == request.UserAccountId)
                .FirstAsync(cancellationToken);

            follower.Unfollow();

            return await _repository.SaveAggregateAsync(follower);
        }
    }
}
