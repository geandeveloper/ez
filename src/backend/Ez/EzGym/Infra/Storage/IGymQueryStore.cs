using EzCommon.Infra.Storage;
using EzGym.Models;
using System.Collections.Generic;

namespace EzGym.Infra.Storage
{
    public interface IGymQueryStore : IQueryStorage
    {
        IList<Follower> GetFollowersByAccountName(string accountName);
    }
}
