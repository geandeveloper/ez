using EzCommon.Infra.Storage;
using EzGym.SnapShots;
using System.Collections.Generic;

namespace EzGym.Infra.Storage
{
    public interface IGymQueryStore : IQueryStorage
    {
        IList<AccountSnapShot> QueryFollowers(string accountName, string query);
        IList<AccountSnapShot> QueryFollowing(string accountName, string query);
    }
}
