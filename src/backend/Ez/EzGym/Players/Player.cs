using System;
using System.Collections.Generic;
using EzCommon.Models;
using EzGym.Events.Player;

namespace EzGym.Players
{
    public class Player : AggregateRoot
    {
        public string AccountId { get; private set; }
        public int Level { get; private set; }
        public IList<PlayerAchievement> Achievements { get; } = new List<PlayerAchievement>();
        public IList<Goal> Goals { get; } = new List<Goal>();

        public Player() { }

        public Player(string accountId)
        {
            RaiseEvent(new PlayerCreateEvent(GenerateNewId(), accountId));
        }

        public void LevelUp()
        {
            RaiseEvent(new PlayerLevelUpEvent(Id, Level));
        }

        public void CompleteGoal(GoalTypeEnum goalType)
        {
            RaiseEvent(new PlayerGoalCompleteEvent(Id, goalType, DateTime.UtcNow));
        }

        protected void Apply(PlayerGoalCompleteEvent @event)
        {
            Goals.Add(new Goal(@event.GoalType, @event.CompleteDateTime));
        }

        protected void Apply(PlayerCreateEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
            Level = 1;
        }

        protected void Apply(PlayerLevelUpEvent @event)
        {
            Level = @event.CurrentLevel + 1;
        }
    }
}
