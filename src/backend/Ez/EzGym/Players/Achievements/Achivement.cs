using EzCommon.Models;
using EzGym.Events.Achievement;

namespace EzGym.Players.Achievements
{
    public class Achievement : AggregateRoot
    {
        public string Name { get; private set; }
        public string ImageUrl { get; private set; }
        public int Level { get; set; }

        public Achievement() { }

        public Achievement(string name, string imageUrl)
        {
            RaiseEvent(new AchievementCreatedEvent(GenerateNewId(), name, imageUrl));
        }

        protected void Apply(AchievementCreatedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            ImageUrl = @event.ImageUrl;
        }
    }
}
