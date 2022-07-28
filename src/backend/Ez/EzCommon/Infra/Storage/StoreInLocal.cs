using EzCommon.Events;
using EzCommon.Infra.Bus;
using EzCommon.Models;
using LiteDB;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage
{
    public class StoreInLocal : IEventStore, IQueryStorage
    {
        static StoreInLocal()
        {
            BsonMapper.Global
                .Entity<EventStream>()
                .DbRef(es => es.EventRows);
        }


        private readonly IBus _bus;

        public StoreInLocal(IBus bus)
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
            }
            else
            {
                eventStreamState.EventRows.AddRange(uncommittedEvents);
                db.GetCollection<EventStream>().Update(eventStreamState);
            }

            db.GetCollection<EventRow>().InsertBulk(uncommittedEvents);

            await _bus.PublishAsync(new SnapShotEvent<T>(aggregate));
            await _bus.PublishAsync(uncommittedEvents.Select(ue => ue.Data).ToArray());
            return new EventStream(eventStreamId, uncommittedEvents.Select(ue => ue.Data).ToList());
        }

        public T GetSnapShot<T>(Expression<Func<T, bool>> query) where T : AggregateRoot
        {
            using var db = new LiteDatabase(@"C:\temp\snapshots.db");
            return db.GetCollection<T>().FindOne(query);
        }

        public T UpinsertSnapShot<T>(T snapShot) where T : AggregateRoot
        {
            using var db = new LiteDatabase(@"C:\temp\snapshots.db");
            db.GetCollection<T>().Upsert(snapShot);
            return snapShot;
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> query)
        {
            using var db = new LiteDatabase(@"C:\temp\snapshots.db");
            return db.GetCollection<T>().Find(query).ToList().AsQueryable();
        }
    }
}
