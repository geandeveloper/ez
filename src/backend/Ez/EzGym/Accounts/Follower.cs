using System;

namespace EzGym.Accounts
{
    public class Follower
    {
        public string AccountId { get; private set; }
        public Account Account { get; set; }

        public Follower(string accountId)
        {
            AccountId = accountId;
        }
    }
}
