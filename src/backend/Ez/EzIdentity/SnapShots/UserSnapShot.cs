using EzCommon.Models;
using EzIdentity.Models;

namespace EzIdentity.SnapShots
{
    public class UserSnapShot : SnapShot
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string Password { get; set; }
        public bool Activated { get; set; }
    }
}
