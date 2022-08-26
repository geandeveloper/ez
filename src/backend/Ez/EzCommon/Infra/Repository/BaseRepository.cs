using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using Marten;

namespace EzCommon.Infra.Repository
{
    public class BaseRepository<TStorage> : IBaseRepository
        where TStorage : IEventStore
    {
        private readonly TStorage _storage;
        private readonly IBus _bus;

        public BaseRepository(TStorage storage, IBus bus)
        {
            _storage = storage;
            _bus = bus;
        }

        public async Task<TAggregate> LoadAggregateAsync<TAggregate>(string aggregateId) where TAggregate : AggregateRoot
        {
            var eventStreamId = new EventStreamId(typeof(TAggregate), aggregateId);
            var aggregate = Activator.CreateInstance<TAggregate>();
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            await using var session = _storage.OpenSession();
            return await session.Events.AggregateStreamAsync<TAggregate>(eventStream.Id);
        }

        public async Task<EventStream> SaveAggregateAsync<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
        {
            var eventStreamId = new EventStreamId(aggregate.GetType(), aggregate.Id);
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            if (!eventStream.GetUncommitedEvents().Any())
                return eventStream;

            await using var session = _storage.OpenSession();
            if (session.Load<TAggregate>(eventStream.Id) == null)
                session.Events.StartStream<TAggregate>(eventStream.Id, eventStream.GetUncommitedEvents().Cast<object>().ToArray());
            else
                await session.Events.AppendOptimistic(eventStream.Id, eventStream.GetUncommitedEvents().Cast<object>().ToArray());

            await session.SaveChangesAsync();

            await _bus.PublishAsync(eventStream.GetUncommitedEvents().ToArray());

            return eventStream;
        }

        public async Task<TAggregate> QueryAsync<TAggregate>(Expression<Func<TAggregate, bool>> query)
        {
            await using var session = _storage.OpenSession();
            return await session.Query<TAggregate>().Where(query).FirstOrDefaultAsync(CancellationToken.None);
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> query)
        {
            using var session = _storage.OpenSession();
            return session.Query<T>().Where(query);
        }

        public T QueryOne<T>(Expression<Func<T, bool>> query)
        {
            using var session = _storage.OpenSession();
            return session.Query<T>().Where(query).FirstOrDefault();
        }
    }
}
