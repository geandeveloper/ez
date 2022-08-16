using EzCommon.Infra.Storage;
using EzGym.Models;
using System.Collections.Generic;

namespace EzGym.Infra.Storage
{
    public interface IGymQueryStore : IQueryStorage
    {
        IList<Account> QueryFollowers(string accountName, string query);
        IList<Account> QueryFollowing(string accountName, string query);
    }
}
