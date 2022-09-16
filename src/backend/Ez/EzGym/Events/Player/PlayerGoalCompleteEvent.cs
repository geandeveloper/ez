using System;
using EzCommon.Events;
using EzGym.Players;

namespace EzGym.Events.Player;

public record PlayerGoalCompleteEvent(string PlayerId, GoalTypeEnum GoalType, DateTime CompleteDateTime) : Event;