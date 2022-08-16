using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzGym.Models;
using LiteDB;
using Marten;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace EzGym.Infra.Storage
{
    public class GymEventStore : MartenStorage, IGymEventStore, IGymQueryStore
    {
        public GymEventStore(IBus bus, IDocumentSession session) : base(bus, session)
        {

        }

        public IList<Account> QueryFollowers(string accountName, string query)
        {
            var account = QueryOne<Account>(a => a.AccountName == accountName);
            return account.Followers.Select(f => QueryOne<Account>(a => a.Id == f.AccountId)).Where(a => a.AccountName.Contains(query)).ToList();
        }

        public IList<Account> QueryFollowing(string accountName, string query)
        {
            var account = QueryOne<Account>(a => a.AccountName == accountName);
            return account.Following.Select(f => QueryOne<Account>(a => a.Id == f.AccountId)).Where(a => a.AccountName.Contains(query)).ToList();
        }
    }
}
