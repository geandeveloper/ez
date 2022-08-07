using EzIdentity.Models;
using System;

namespace EzIdentity.SnapShots
{
    public class UserSnapShot
    {
        public int Version { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string Password { get; set; }
        public bool Activated { get; set; }
    }
}
