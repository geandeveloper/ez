using System;
using System.Collections.Generic;

namespace EzGym.Gyms
{
    public class GymUser
    {
        public Guid GymId { get; private set; }
        public Guid? AccountId { get; private set; }

        public string Name { get; private set; }
        public int? Age { get; private set; }
        public string AvatarUrl { get; set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }

        public IEnumerable<GymMemberShip> MemberShips { get; private set; }

    }
}
