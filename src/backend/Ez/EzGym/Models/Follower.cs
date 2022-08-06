using System;

namespace EzGym.Models
{
    public class Follower
    {
        public Guid AccountId { get; private set; }
        public Account Account { get;  set; }

        public Follower(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
