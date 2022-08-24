using System;

namespace EzGym.Gyms.Users
{
    public record GymMemberShip
    {
        public string Id { get; private init; }
        public string PlanId { get; private init; }
        public string PaymentId { get; private init; }
        public bool Active { get; private init; }
        public decimal Price { get; private init; }
        public int Days { get; private init; }
        public DateTime PurchaseDateTime { get; private init; }
        public DateTime? PaymentDateTime { get; } = null;

        public static GymMemberShip CreatePendingMemberShip(string id, string planId, string paymentId, decimal price, int days) => new()
        {
            Id = id,
            PlanId = planId,
            PaymentId = paymentId,
            Active = false,
            Price = price,
            Days = days,
            PurchaseDateTime = DateTime.UtcNow
        };
    }
}
