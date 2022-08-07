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
    public abstract class StoreInLocal : IEventStore, IQueryStorage
    {

        private readonly ILiteDatabase _db;

        static StoreInLocal()
        {
            BsonMapper.Global
                .Entity<EventStream>()
                .DbRef(es => es.EventRows);
        }


        private readonly IBus _bus;
        protected readonly string _storeName;

        public StoreInLocal(IBus bus, string storeName)
        {
            _bus = bus;
            _storeName = storeName;

            var connection = new ConnectionString($"C:/temp/{_storeName}.db")
            {
                Connection = ConnectionType.Shared
            };

            _db = new LiteDatabase(connection);
        }

        public async Task<EventStream> SaveAsync<T,TSnapShot>(T aggregate)
            where T : AggregateRoot, ISnapShotManager<T, TSnapShot>
        {
            var eventStreamId = new EventStreamId(aggregate.GetType(), aggregate.Id);
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            var eventStreamState = _db.GetCollection<EventStream>().FindById(eventStream.Id);
            var uncommittedEvents = eventStream.EventRows.Where(e => e.Version > (eventStreamState?.Version ?? 0)).ToList();

            if (eventStreamState == null)
            {
                _db.GetCollection<EventStream>().Insert(eventStream);
            }
            else
            {
                eventStreamState.EventRows.AddRange(uncommittedEvents);
                _db.GetCollection<EventStream>().Update(eventStreamState);
            }

            _db.GetCollection<EventRow>().InsertBulk(uncommittedEvents);

            await _bus.PublishAsync(uncommittedEvents.Select(ue => ue.Data).ToArray());
            await _bus.PublishAsync(new SnapShotEvent<TSnapShot>(aggregate.ToSnapShot()));

            return new EventStream(eventStreamId, uncommittedEvents.Select(ue => ue.Data).ToList());
        }

        public T GetSnapShot<T>(Expression<Func<T, bool>> query)
        {
            using var db = new LiteDatabase($"C:/temp/{_storeName}-snapshots.db");
            return db.GetCollection<T>().FindOne(query);
        }

        public T UpinsertSnapShot<T>(T snapShot)
        {
            using var db = new LiteDatabase($"C:/temp/{_storeName}-snapshots.db");
            db.GetCollection<T>().Upsert(snapShot);
            return snapShot;
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> query)
        {
            using var db = new LiteDatabase($"C:/temp/{_storeName}-snapshots.db");
            return db.GetCollection<T>().Find(query).ToList().AsQueryable();
        }

    }
}
