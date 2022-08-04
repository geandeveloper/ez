using System;

namespace EzGym.Models
{
    public class Follower
    {
        public Guid AccountId { get; private set; }

        private Follower() { }
        public Follower(Account account)
        {
            AccountId = account.Id;
        }
    }
}
