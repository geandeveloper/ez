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
        private readonly IGymQueryStore _queryStorage;

        public UpInsertAccountProfileCommandHandler(IGymEventStore eventStorage, IGymQueryStore queryStorage)
        {
            _eventStorage = eventStorage;
            _queryStorage = queryStorage;
        }

        public Task<EventStream> Handle(UpInsertAccountProfileCommand request, CancellationToken cancellationToken)
        {
            var snapShot = _queryStorage.GetSnapShot<AccountSnapShot>(p => p.Id == request.AccountId);
            var account = Account.RestoreSnapShot(snapShot);

            account.UpdateProfile(request);

            return _eventStorage.SaveAsync<Account, AccountSnapShot>(account);
        }
    }
}
