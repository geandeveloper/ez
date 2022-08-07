using EzGym.Models;
using System;
using System.Collections.Generic;

namespace EzGym.SnapShots
{
    public class AccountSnapShot
    {
        public int Version { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AccountName { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public bool IsDefault { get; set; }
        public string AvatarUrl { get; set; }
        public Profile Profile { get; set; }
        public IList<Follower> Following { get; set; }
        public IList<Follower> Followers { get; set; }
        public int? FollowingCount => Following?.Count;
        public int? FollowersCount => Followers?.Count;
    }
}
