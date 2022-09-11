using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;

namespace EzGym.Accounts.Followers.FollowAccount
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
            var follower = new Follower(request.FollowAccountId, request.UserAccountId);

            return await _repository.SaveAggregateAsync(follower);
        }
    }
}
