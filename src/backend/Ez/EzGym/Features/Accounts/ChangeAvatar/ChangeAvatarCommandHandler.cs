using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Accounts.ChangeAvatar
{
    public class ChangeAvatarCommandHandler : ICommandHandler<ChangeAvatarCommand>
    {
        private readonly IFileStorage _fileStorage;
        private readonly IGymEventStore _eventStore;

        public ChangeAvatarCommandHandler(
            IFileStorage fileStorage,
            IGymEventStore eventStore)
        {
            _fileStorage = fileStorage;
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
        {
            var account = await _eventStore.LoadAggregateAsync<Account>(request.AccountId);
            var avatarUrl = await _fileStorage.UploadFileAsync(request.AvatarStream, account.Id.ToString(), Path.GetExtension(request.FileName));

            account.ChangeAvatarImage(avatarUrl);

            return await _eventStore.SaveAggregateAsync(account);
        }
    }
}
