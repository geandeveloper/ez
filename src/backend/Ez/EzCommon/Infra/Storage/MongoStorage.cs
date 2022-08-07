using EzCommon.Events;
using EzCommon.Infra.Bus;
using EzCommon.Models;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage
{
    public class MongoStorage : IEventStore
    {
        private readonly IMongoDatabase _db;
        private readonly IBus _bus;

        public MongoStorage(IBus bus, string databaseName)
        {
            _bus = bus;
            _db = new MongoClient("mongodb://127.0.0.1:27017")
                .GetDatabase(databaseName);
        }

        public async Task<EventStream> SaveAsync<T, TSnapShot>(T aggregate) where T : AggregateRoot, ISnapShotManager<T, TSnapShot>
        {
            var eventStreamId = new EventStreamId(aggregate.GetType(), aggregate.Id);
            var eventStream = new EventStream(eventStreamId, aggregate.GetEvents().ToList());

            var eventStreamCollection = _db.GetCollection<EventStream>(nameof(EventStream));
            var eventStreamDbState = await eventStreamCollection.Find(e => e.Id == aggregate.Id.ToString()).FirstOrDefaultAsync();
            var uncommitedEvents = eventStream.GetUncommitedEvents(eventStreamDbState?.Version);

            var newEventStreamState = eventStreamDbState?.CommitEvents(uncommitedEvents) ?? eventStream;

            eventStreamCollection.ReplaceOne(
                e => e.Id == eventStream.Id,
                newEventStreamState,
                new ReplaceOptions { IsUpsert = true });


            var eventRowCollection = _db.GetCollection<EventRow>(nameof(EventRow));
            eventRowCollection.InsertMany(uncommitedEvents);

            await _bus.PublishAsync(uncommitedEvents.Select(ue => ue.Data).ToArray());
            await _bus.PublishAsync(new SnapShotEvent<TSnapShot>(aggregate.ToSnapShot()));
            return new EventStream(eventStreamId, uncommitedEvents.Select(ue => ue.Data).ToList());

        }
    }
}
