using EzCommon.Infra.Bus;
using EzCommon.Models;
using Marten;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage
{
    public abstract class MartenStorage : IEventStore
    {
        private readonly IDocumentSession _session;
        private readonly IBus _bus;

        public MartenStorage(IBus bus, IDocumentSession session)
        {
            _bus = bus;
            _session = session;
        }

        public async Task<TAggregate> LoadAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : AggregateRoot
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
                _session.Events.StartStream<TAggregate>(eventStream.Id, eventStream.GetUncommitedEvents().ToArray());
            else
                await _session.Events.AppendOptimistic(eventStream.Id, eventStream.GetUncommitedEvents().ToArray());

            await _session.SaveChangesAsync();

            await _bus.PublishAsync(eventStream.GetUncommitedEvents().ToArray());

            return eventStream;
        }

        public Task<TAggregate> QueryAsync<TAggregate>(Expression<Func<TAggregate, bool>> query)
        {
            return _session.Query<TAggregate>().SingleAsync(query);
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> query)
        {

            return _session.Query<T>().Where(query);
        }

        public T QueryOne<T>(Expression<Func<T, bool>> query)
        {
            return _session.Query<T>().Where(query).FirstOrDefault();
        }

    }
}
