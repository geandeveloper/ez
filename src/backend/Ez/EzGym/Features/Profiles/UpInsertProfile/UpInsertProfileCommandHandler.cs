using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Profiles.UpInsertProfile
{
    public class UpInsertProfileCommandHandler : ICommandHandler<UpInsertProfileCommand>
    {
        private readonly IGymEventStore _eventStorage;
        private readonly IGymQueryStore _queryStorage;

        public UpInsertProfileCommandHandler(IGymEventStore eventStorage, IGymQueryStore queryStorage)
        {
            _eventStorage = eventStorage;
            _queryStorage = queryStorage;
        }

        public Task<EventStream> Handle(UpInsertProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = _queryStorage.GetSnapShot<Profile>(p => p.AccountId == request.AccountId);

            if (profile != null)
            {
                profile.UpdateProfile(request);
                return _eventStorage.SaveAsync(profile);
            }

            return _eventStorage.SaveAsync(new Profile(request));



        }
    }
}
