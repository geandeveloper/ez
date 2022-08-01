using EzCommon.EventHandlers;
using EzCommon.Events;
using EzCommon.Infra.Storage;
using EzGym.Infra.Storage;
using EzGym.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.EventHandlers
{
    public class UpdateSnapShotsEventHandler :
        IEventHandler<SnapShotEvent<Gym>>,
        IEventHandler<SnapShotEvent<Account>>,
        IEventHandler<SnapShotEvent<Profile>>
    {
        private readonly IQueryStorage _queryStorage;

        public UpdateSnapShotsEventHandler(IGymQueryStorage queryStorage)
        {
            _queryStorage = queryStorage;
        }

        public Task Handle(SnapShotEvent<Gym> notification, CancellationToken cancellationToken)
        {
            _queryStorage.UpinsertSnapShot(notification.SnapShot);
            return Task.CompletedTask;
        }

        public Task Handle(SnapShotEvent<Account> notification, CancellationToken cancellationToken)
        {
            _queryStorage.UpinsertSnapShot(notification.SnapShot);
            return Task.CompletedTask;
        }

        public Task Handle(SnapShotEvent<Profile> notification, CancellationToken cancellationToken)
        {
            _queryStorage.UpinsertSnapShot(notification.SnapShot);
            return Task.CompletedTask;
        }
    }
}
