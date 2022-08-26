using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EzCommon.Models;

namespace EzCommon.Infra.Repository
{
    public interface IBaseRepository
    {
        Task<TAggregate> LoadAggregateAsync<TAggregate>(string aggregateId) where TAggregate : AggregateRoot;

        Task<EventStream> SaveAggregateAsync<TAggregate>(TAggregate aggregate)
          where TAggregate : AggregateRoot;

        Task<TAggregate> QueryAsync<TAggregate>(Expression<Func<TAggregate, bool>> query);

        T QueryOne<T>(Expression<Func<T, bool>> query);

        IQueryable<T> Where<T>(Expression<Func<T, bool>> query);

    }
}
