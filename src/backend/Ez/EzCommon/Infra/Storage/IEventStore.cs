
using EzCommon.Models;
using System.Linq.Expressions;

namespace EzCommon.Infra.Storage;

public interface IEventStore
{
   T GetSnapShot<T>(Expression<Func<T, bool>> query) where T : AggregateRoot;
    Task<EventStream> SaveAsync<T>(T aggregate) where T : AggregateRoot;
    T GetById<T>(Guid id) where T : AggregateRoot, new();
}

