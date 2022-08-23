using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;

namespace EzGym.Accounts.UpInsertAccountProfile
{
    public class UpInsertAccountProfileCommandHandler : ICommandHandler<UpInsertAccountProfileCommand>
    {
        private readonly IGymEventStore _eventStorage;

        public UpInsertAccountProfileCommandHandler(IGymEventStore eventStorage, IGymQueryStore queryStorage)
        {
            _eventStorage = eventStorage;
        }

        public async Task<EventStream> Handle(UpInsertAccountProfileCommand request, CancellationToken cancellationToken)
        {
            var account = await _eventStorage.LoadAggregateAsync<Account>(request.AccountId);
            account.UpdateProfile(request);

            return await _eventStorage.SaveAggregateAsync(account);
        }
    }
}
