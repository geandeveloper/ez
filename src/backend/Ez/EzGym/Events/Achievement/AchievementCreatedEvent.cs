using EzCommon.Events;

namespace EzGym.Events.Achievement;

public record AchievementCreatedEvent(string Id, string Name, string ImageUrl) : Event;