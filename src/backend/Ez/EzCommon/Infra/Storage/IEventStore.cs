
using EzCommon.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage;

public interface IEventStore
{
    T GetSnapShot<T>(Expression<Func<T, bool>> query) where T : AggregateRoot;
    Task<EventStream> SaveAsync<T>(T aggregate) where T : AggregateRoot;
}

