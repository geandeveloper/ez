using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;

namespace EzGym.Accounts.UpInsertAccountProfile
{
    public class UpInsertAccountProfileCommandHandler : ICommandHandler<UpInsertAccountProfileCommand>
    {
        private readonly IGymRepository _repository;

        public UpInsertAccountProfileCommandHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventStream> Handle(UpInsertAccountProfileCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.LoadAggregateAsync<Account>(request.AccountId);
            account.UpdateProfile(request);

            return await _repository.SaveAggregateAsync(account);
        }
    }
}
