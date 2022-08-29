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
    public class BaseRepository<TStorage> : IBaseRepository, IDisposable
        where TStorage : IEventStore
    {
        private readonly IDocumentSession _session;
        private readonly IBus _bus;

        public BaseRepository(TStorage storage, IBus bus)
        {
            _session = storage.OpenSession();
            _bus = bus;
        }

        public async Task<TAggregate> LoadAggregateAsync<TAggregate>(string aggregateId) where TAggregate : AggregateRoot
        {
            var eventStreamId = new EventStreamId(typeof(TAggregate), aggregateId);
            var aggregate = Activator.CreateInstance<TAggregate>();
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            return await _session.Events.AggregateStreamAsync<TAggregate>(eventStream.Id);
        }

        public async Task<EventStream> SaveAggregateAsync<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
        {
            var eventStreamId = new EventStreamId(aggregate.GetType(), aggregate.Id);
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            if (!eventStream.GetUncommitedEvents().Any())
                return eventStream;

            if (_session.Load<TAggregate>(eventStream.Id) == null)
                _session.Events.StartStream<TAggregate>(eventStream.Id, eventStream.GetUncommitedEvents().Cast<object>().ToArray());
            else
                await _session.Events.AppendOptimistic(eventStream.Id, eventStream.GetUncommitedEvents().Cast<object>().ToArray());

            await _session.SaveChangesAsync();

            await _bus.PublishAsync(eventStream.GetUncommitedEvents().ToArray());

            return eventStream;
        }

        public async Task<TAggregate> QueryAsync<TAggregate>(Expression<Func<TAggregate, bool>> query)
        {
            return await _session.Query<TAggregate>().Where(query).FirstOrDefaultAsync(CancellationToken.None);
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> query)
        {
            return _session.Query<T>().Where(query);
        }

        public T QueryOne<T>(Expression<Func<T, bool>> query)
        {
            return _session.Query<T>().Where(query).FirstOrDefault();
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
