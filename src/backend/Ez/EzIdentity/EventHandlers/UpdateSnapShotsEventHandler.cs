using EzCommon.EventHandlers;
using EzCommon.Events;
using EzCommon.Infra.Storage;
using EzIdentity.Infra.Storage;
using EzIdentity.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.EventHandlers
{
    public class UpdateSnapShotsEventHandler : 
        IEventHandler<SnapShotEvent<User>>
    {
        private readonly IQueryStorage _queryStorage;

        public UpdateSnapShotsEventHandler(IIdentityQueryStore queryStorage)
        {
            _queryStorage = queryStorage;
        }

        public Task Handle(SnapShotEvent<User> notification, CancellationToken cancellationToken)
        {
            _queryStorage.UpinsertSnapShot(notification.SnapShot);
            return Task.CompletedTask;
        }
    }
}
