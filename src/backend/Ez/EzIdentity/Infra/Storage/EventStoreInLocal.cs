using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using LiteDB;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzIdentity.Infra.Storage
{
    public class EventStoreInLocal : IEventStore
    {
        static EventStoreInLocal()
        {
            BsonMapper.Global
                .Entity<EventStream>()
                .DbRef(es => es.EventRows);
        }


        private readonly IBus _bus;

        public EventStoreInLocal(IBus bus)
        {
            _bus = bus;
        }

        public T GetById<T>(Guid id)
            where T : AggregateRoot, new()
        {
            var aggregate = new T();
            var streamId = new EventStreamId(aggregate.GetType(), id);

            using var db = new LiteDatabase(@"C:\temp\identity.db");
            var eventStream = db.GetCollection<EventStream>().Include(es => es.EventRows).FindById(streamId.ToString());
            aggregate.LoadFromEvents(eventStream.EventRows.Select(er => er.Data).OrderBy(e => e.Version));

            return aggregate;
        }

        public async Task<EventStream> SaveAsync<T>(T aggregate)
            where T : AggregateRoot
        {
            var eventStreamId = new EventStreamId(aggregate.GetType(), aggregate.Id);
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            using var db = new LiteDatabase(@"C:\temp\identity.db");
            var currentEventStreamState = db.GetCollection<EventStream>().FindById(eventStream.Id);
            var uncommittedEvents = eventStream.EventRows.Where(e => e.Version > (currentEventStreamState?.Version ?? 0)).ToList();

            if (currentEventStreamState == null)
            {
                db.GetCollection<EventStream>().Insert(eventStream);
                db.GetCollection<T>().Insert(aggregate);
            }
            else
            {
                db.GetCollection<EventStream>().Update(eventStream);
                db.GetCollection<T>().Update(aggregate);
            }

            db.GetCollection<EventRow>().InsertBulk(uncommittedEvents);

            await _bus.PublishAsync(uncommittedEvents.Select(ue => ue.Data).ToArray());
            return new EventStream(eventStreamId, uncommittedEvents.Select(ue => ue.Data).ToList());
        }

        public T GetSnapShot<T>(Expression<Func<T, bool>> query) where T : AggregateRoot
        {
            using var db = new LiteDatabase(@"C:\temp\identity.db");
            return db.GetCollection<T>().FindOne(query);
        }
    }
}
