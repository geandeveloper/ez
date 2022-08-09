using EzCommon.EventHandlers;
using EzCommon.Events;
using EzGym.Infra.Storage;
using EzGym.SnapShots;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.EventHandlers
{
    public class UpdateSnapShotEventHandler : IEventHandler<SnapShotEvent<AccountSnapShot>>
    {
        private readonly IGymQueryStore _queryStore;

        public UpdateSnapShotEventHandler(IGymQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public Task Handle(SnapShotEvent<AccountSnapShot> notification, CancellationToken cancellationToken)
        {
            _queryStore.UpInsert(a => a.Id == notification.Value.Id, notification.Value);
            return Task.CompletedTask;
        }
    }
}
