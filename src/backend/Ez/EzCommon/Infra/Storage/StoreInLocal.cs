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
        private readonly MongoStorage _mongoStorage;

        static StoreInLocal()
        {
        }


        protected readonly string _storeName;

        public StoreInLocal(IBus bus, string storeName)
        {
            _storeName = storeName;

            var connection = new ConnectionString($"C:/temp/{_storeName}.db")
            {
                Connection = ConnectionType.Shared
            };

            _mongoStorage = new MongoStorage(bus, storeName);

            _db = new LiteDatabase(connection);
        }

        public Task<EventStream> SaveAsync<T, TSnapShot>(T aggregate)
            where T : AggregateRoot, ISnapShotManager<T, TSnapShot>
        {
            return _mongoStorage.SaveAsync<T, TSnapShot>(aggregate);
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
