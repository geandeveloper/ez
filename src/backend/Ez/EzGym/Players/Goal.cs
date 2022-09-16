using System;

namespace EzGym.Players
{
    public class Goal
    {
        public GoalTypeEnum GoalType { get; }
        public DateTime CompleteDateTime { get; }

        public Goal(GoalTypeEnum goalType, DateTime completeDateTime)
        {
            GoalType = goalType;
            CompleteDateTime = completeDateTime;
        }
    }
}
