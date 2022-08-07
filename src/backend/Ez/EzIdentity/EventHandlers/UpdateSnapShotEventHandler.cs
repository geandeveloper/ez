using EzCommon.EventHandlers;
using EzCommon.Events;
using EzIdentity.Infra.Storage;
using EzIdentity.SnapShots;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.EventHandlers
{
    public class UpdateSnapShotEventHandler : IEventHandler<SnapShotEvent<UserSnapShot>>
    {
        private readonly IIdentityQueryStore _queryStore;

        public UpdateSnapShotEventHandler(IIdentityQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public Task Handle(SnapShotEvent<UserSnapShot> notification, CancellationToken cancellationToken)
        {
            _queryStore.UpinsertSnapShot(notification.Value);
            return Task.CompletedTask;
        }
    }
}
