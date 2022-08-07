using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using EzGym.SnapShots;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Accounts.ChangeAvatar
{
    public class ChangeAvatarCommandHandler : ICommandHandler<ChangeAvatarCommand>
    {
        private readonly IGymQueryStore _queryStorage;
        private readonly IFileStorage _fileStorage;
        private readonly IGymEventStore _eventStore;

        public ChangeAvatarCommandHandler(
            IGymQueryStore queryStorage,
            IFileStorage fileStorage,
            IGymEventStore eventStore)
        {
            _queryStorage = queryStorage;
            _fileStorage = fileStorage;
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
        {
            var snapShot = _queryStorage.Query<AccountSnapShot>(a => a.UserId == request.UserId && a.Id == request.AccountId).FirstOrDefault();
            var account = Account.RestoreSnapShot(snapShot);
            var avatarUrl = await _fileStorage.UploadFileAsync(request.AvatarStream, account.Id.ToString(), Path.GetExtension(request.FileName));

            account.ChangeAvatarImage(avatarUrl);

            return await _eventStore.SaveAsync<Account, AccountSnapShot>(account);
        }
    }
}
