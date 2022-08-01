using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Profiles.ChangeAvatar
{
    public class ChangeAvatarCommandHandler : ICommandHandler<ChangeAvatarCommand>
    {
        private readonly IGymQueryStorage _queryStorage;
        private readonly IFileStorage _fileStorage;

        public ChangeAvatarCommandHandler(IGymQueryStorage queryStorage, IFileStorage fileStorage)
        {
            _queryStorage = queryStorage;
            _fileStorage = fileStorage;
        }

        public async Task<EventStream> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
        {
            var account = _queryStorage.Query<Account>(a => a.UserId == request.UserId && a.Id == request.AccountId).FirstOrDefault();
            await _fileStorage.UploadFileAsync(request.AvatarStream, account.Id.ToString());
            return account.ToEventStream();
        }
    }
}
