using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using EzGym.SnapShots;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Accounts.UpInsertAccountProfile
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
            var account = await _eventStorage.QueryLatestVersionAsync<AccountSnapShot, Account>(p => p.Id == request.AccountId);
            account.UpdateProfile(request);

            return await _eventStorage.SaveAsync<Account, AccountSnapShot>(account);
        }
    }
}
