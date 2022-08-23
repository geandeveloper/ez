using System;

namespace EzGym.Gyms
{
    public class GymPlan
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int Days { get; init; }
        public decimal Price { get; init; }
        public bool Active { get; init; }

        public GymPlan(Guid id, string name, int days, decimal price, bool active)
        {
            Id = id;
            Name = name;
            Days = days;
            Price = price;
            Active = active;
        }
    }
}
