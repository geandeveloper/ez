using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using LiteDB;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage
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

        public async Task<EventStream> SaveAsync<T>(T aggregate)
            where T : AggregateRoot
        {
            var eventStreamId = new EventStreamId(aggregate.GetType(), aggregate.Id);
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            using var db = new LiteDatabase(@"C:\temp\identity.db");
            var eventStreamState = db.GetCollection<EventStream>().FindById(eventStream.Id);
            var uncommittedEvents = eventStream.EventRows.Where(e => e.Version > (eventStreamState?.Version ?? 0)).ToList();

            if (eventStreamState == null)
            {
                db.GetCollection<EventStream>().Insert(eventStream);
                db.GetCollection<T>().Insert(aggregate);
            }
            else
            {
                eventStreamState.EventRows.AddRange(uncommittedEvents);
                db.GetCollection<EventStream>().Update(eventStreamState);
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
