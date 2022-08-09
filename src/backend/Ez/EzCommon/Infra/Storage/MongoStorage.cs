using EzCommon.Events;
using EzCommon.Infra.Bus;
using EzCommon.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage
{
    public class MongoStorage : IEventStore, IQueryStorage
    {
        protected readonly IMongoDatabase _db;
        protected readonly IMongoClient _client;
        private readonly IBus _bus;

        public MongoStorage(IBus bus, string databaseName)
        {
            _bus = bus;
            _client = new MongoClient("mongodb://127.0.0.1:27017");
            _db = _client.GetDatabase(databaseName);
        }

        public T QueryOne<T>(Expression<Func<T, bool>> query)
        {
            return _db.GetCollection<T>(typeof(T).Name).Find(query).First();
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> query)
        {
            return _db.GetCollection<T>(typeof(T).Name).Find(query).ToList().AsQueryable();
        }

        public T UpInsert<T>(Expression<Func<T, bool>> query, T model)
        {
            var collection = _db.GetCollection<T>(typeof(T).Name);
            collection.ReplaceOne(
                query,
                model,
                new ReplaceOptions
                {
                    IsUpsert = true
                });

            return model;
        }

        public async Task<TAggregate> QueryLatestVersionAsync<TSnapShot, TAggregate>(Expression<Func<TSnapShot, bool>> querySnapShot)
            where TAggregate : AggregateRoot, ISnapShotManager<TAggregate, TSnapShot>
            where TSnapShot : SnapShot
        {
            var snapShot = QueryOne(querySnapShot);
            var aggregateSnapShot = Activator.CreateInstance<TAggregate>().FromSnapShot(snapShot);

            var eventStreamId = new EventStreamId(snapShot.GetType(), snapShot.Id);
            var eventStreamCollection = _db.GetCollection<EventStream>(nameof(EventStream));
            var eventStreamDbState = await eventStreamCollection.Find(e => e.Id == eventStreamId.ToString()).FirstOrDefaultAsync();


            var eventRowCollection = _db.GetCollection<EventRow>(nameof(EventRow));
            var latestEvents = eventRowCollection.Find(e => e.Version > aggregateSnapShot.Version).ToList();

            aggregateSnapShot.LoadFromEvents(latestEvents.Select(e => e.Data));

            return aggregateSnapShot;
        }

        public async Task<EventStream> SaveAsync<T, TSnapShot>(T aggregate) where T : AggregateRoot, ISnapShotManager<T, TSnapShot>
        {
            var eventStreamId = new EventStreamId(aggregate.GetType(), aggregate.Id);
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            if (!eventStream.GetUncommitedEvents().Any())
                return eventStream;

            var eventStreamCollection = _db.GetCollection<EventStream>(nameof(EventStream));
            var eventRowCollection = _db.GetCollection<EventRow>(nameof(EventRow));

            var eventStreamDbState = await eventStreamCollection.Find(e => e.Id == eventStreamId.ToString()).FirstOrDefaultAsync();
            var uncommitedEvents = eventStream.GetUncommitedEvents(eventStreamDbState?.Version).ToList();

            if (eventStreamDbState == null)
                eventStreamCollection.InsertOne(eventStream);
            else
                eventStreamCollection.ReplaceOne(
                    e => e.Id == eventStream.Id,
                    eventStreamDbState?.CommitEvents(uncommitedEvents));


            foreach (var @event in uncommitedEvents)
            {
                await eventRowCollection.ReplaceOneAsync(e => e.Id == @event.Id, @event, new ReplaceOptions { IsUpsert = true });
                await _bus.PublishAsync(@event.Data);
            }

            await _bus.PublishAsync(new SnapShotEvent<TSnapShot>(aggregate.ToSnapShot()));

            return new EventStream(eventStreamId, uncommitedEvents.Select(ue => ue.Data).ToList());
        }
    }
}
