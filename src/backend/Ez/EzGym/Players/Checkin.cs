
using System;
using EzCommon.Models;
using EzGym.Events.Player;

namespace EzGym.Players
{
    public class CheckIn : AggregateRoot
    {
        public string PlayerId { get; private set; }
        public string GymAccountId { get; private set; }
        public string MemberShipId { get; private set; }
        public DateTime CreateAt { get; private set; }
        public DateTime? CompleteAt { get; private set; }


        public CheckIn() {}

        public CheckIn(string playerId, string gymAccountId, string memberShipId)
        {
          RaiseEvent(new CheckInCreatedEvent(GenerateNewId(), playerId, gymAccountId, memberShipId, DateTime.UtcNow));
        }

        protected void Apply(CheckInCreatedEvent @event) {
          Id = @event.Id;
          PlayerId = @event.PlayerId;
          GymAccountId = @event.GymAccountId;
          MemberShipId = @event.MemberShipId;
          CreateAt = @event.CreateAt;
          CompleteAt = null;
        }
    }
}
