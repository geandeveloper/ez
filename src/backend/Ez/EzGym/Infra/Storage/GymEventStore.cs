using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzGym.Models;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace EzGym.Infra.Storage
{
    public class GymEventStore : StoreInLocal, IGymEventStore, IGymQueryStore
    {
        public GymEventStore(IBus bus) : base(bus, "ezgym")
        {
            BsonMapper.Global
                .Entity<Account>()
                .DbRef(es => es.Followers)
                .DbRef(es => es.Following);

            BsonMapper.Global
                .Entity<Follower>()
                .DbRef(es => es.Account)
                .Id(es => es.AccountId);
        }

        public IList<Follower> GetFollowersByAccountName(string accountName)
        {
            using var db = new LiteDatabase($"C:/temp/{_storeName}-snapshots.db");
            var account = db.GetCollection<Account>().FindOne(a => a.AccountName == accountName);

            return account.Followers.Select(f =>
            {
                return new Follower(f.AccountId) { Account = db.GetCollection<Account>().FindById(f.AccountId) };

            }).ToList();
        }
    }
}
