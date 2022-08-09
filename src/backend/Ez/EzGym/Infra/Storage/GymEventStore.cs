using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzGym.Models;
using EzGym.SnapShots;
using LiteDB;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace EzGym.Infra.Storage
{
    public class GymEventStore : MongoStorage, IGymEventStore, IGymQueryStore
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

        public IList<AccountSnapShot> QueryFollowers(string accountName, string query)
        {
            var account = QueryOne<AccountSnapShot>(a => a.AccountName == accountName);
            return account.Followers.Select(f => QueryOne<AccountSnapShot>(a => a.Id == f.AccountId)).Where(a => a.AccountName.Contains(query)).ToList();
        }

        public IList<AccountSnapShot> QueryFollowing(string accountName, string query)
        {
            var account = QueryOne<AccountSnapShot>(a => a.AccountName == accountName);
            return account.Following.Select(f => QueryOne<AccountSnapShot>(a => a.Id == f.AccountId)).Where(a => a.AccountName.Contains(query)).ToList();
        }
    }
}
