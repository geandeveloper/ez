
using EzCommon.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage;

public interface IEventStore
{
    Task<EventStream> SaveAsync<T, TSnapShot>(T aggregate)
      where T : AggregateRoot, ISnapShotManager<T, TSnapShot>;

    Task<TAggregate> QueryLatestVersionAsync<TSnapShot, TAggregate>(Expression<Func<TSnapShot, bool>> querySnapShot)
            where TAggregate : AggregateRoot, ISnapShotManager<TAggregate, TSnapShot>
            where TSnapShot : SnapShot;
}

