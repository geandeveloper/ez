using EzCommon.Models;
using EzGym.Gyms.Events;

namespace EzGym.Gyms
{
    public class GymPlan : AggregateRoot
    {
        public string GymId { get; set; }
        public string Name { get; private init; }
        public int Days { get; private init; }
        public decimal Price { get; private init; }
        public bool Active { get; private init; }

        public GymPlan() { }

        public static GymPlan CreateFromEvent(PlanCreatedEvent @event)
        {
            return new GymPlan
            {
                Id = @event.Id,
                GymId = @event.Command.GymId,
                Name = @event.Command.Name,
                Days = @event.Command.Days,
                Price = @event.Command.Price,
                Active = @event.Command.Active,
            };
        }
    }
}
