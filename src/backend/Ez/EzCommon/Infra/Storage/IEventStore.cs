
using EzCommon.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage;

public interface IEventStore
{
    Task<TAggregate> LoadAggregateAsync<TAggregate>(Guid aggregateId)
      where TAggregate : AggregateRoot;

    Task<EventStream> SaveAggregateAsync<TAggregate>(TAggregate aggregate)
      where TAggregate : AggregateRoot;

    Task<TAggregate> QueryAsync<TAggregate>(Expression<Func<TAggregate, bool>> query);

}

