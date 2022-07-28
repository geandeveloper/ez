using EzCommon.EventHandlers;
using EzCommon.Events;
using EzCommon.Infra.Storage;
using EzGym.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.EventHandlers
{
    public class UpdateSnapShotsEventHandler : IEventHandler<SnapShotEvent<Gym>>
    {
        private readonly IQueryStorage _queryStorage;

        public UpdateSnapShotsEventHandler(IQueryStorage queryStorage)
        {
            _queryStorage = queryStorage;
        }

        public Task Handle(SnapShotEvent<Gym> notification, CancellationToken cancellationToken)
        {
            _queryStorage.UpinsertSnapShot(notification.SnapShot);
            return Task.CompletedTask;
        }
    }
}
