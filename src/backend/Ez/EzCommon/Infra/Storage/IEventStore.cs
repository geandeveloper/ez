
using EzCommon.Models;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage;

public interface IEventStore
{
    Task<EventStream> SaveAsync<T, TSnapShot>(T aggregate)
      where T : AggregateRoot, ISnapShotManager<T, TSnapShot>;
}

