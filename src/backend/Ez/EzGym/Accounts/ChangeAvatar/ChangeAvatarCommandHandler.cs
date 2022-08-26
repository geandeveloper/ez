using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;

namespace EzGym.Accounts.ChangeAvatar
{
    public class ChangeAvatarCommandHandler : ICommandHandler<ChangeAvatarCommand>
    {
        private readonly IFileStorage _fileStorage;
        private readonly IGymRepository _repository;

        public ChangeAvatarCommandHandler(
            IFileStorage fileStorage,
            IGymRepository repository)
        {
            _fileStorage = fileStorage;
            _repository = repository;
        }

        public async Task<EventStream> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.LoadAggregateAsync<Account>(request.AccountId);
            var avatarUrl = await _fileStorage.UploadFileAsync(request.AvatarStream, account.Id.ToString(), Path.GetExtension(request.FileName));

            account.ChangeAvatarImage(avatarUrl);

            return await _repository.SaveAggregateAsync(account);
        }
    }
}
