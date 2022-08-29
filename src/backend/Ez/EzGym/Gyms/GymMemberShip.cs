using System;
using EzCommon.Models;
using EzGym.Events.Gym;

namespace EzGym.Gyms
{
    public class GymMemberShip : AggregateRoot
    {
        public string PayerAccountId { get; private set; }
        public string ReceiverAccountId { get; private set; }
        public string PlanId { get; private set; }
        public string PaymentId { get; private set; }
        public long Amount { get; private set; }
        public int Days { get; private set; }
        public bool Active { get; private set; }
        public DateTime PurchaseDateTime { get; private set; }

        public DateTime? PaymentDateTime { get; private set; }

        public GymMemberShip() { }

        public GymMemberShip(string receiverAccountId, string payerAccountId, string planId, string paymentId, long amount, int days)
        {
            RaiseEvent(new GymMemberShipCreatedEvent(
                Id: GenerateNewId(),
                ReceiverAccountId: receiverAccountId,
                PayerAccountId: payerAccountId,
                PurchaseDateTime: DateTime.UtcNow,
                PlanId: planId,
                PaymentId: paymentId,
                Amount: amount,
                Days: days
                )
            );
        }

        public void Paid()
        {
            RaiseEvent(new GymMemberShipPaidEvent(Id, DateTime.UtcNow));
        }

        protected void Apply(GymMemberShipPaidEvent @event)
        {
            PaymentDateTime = @event.PaymentDateTime;
        }

        protected void Apply(GymMemberShipCreatedEvent @event)
        {
            Id = @event.Id;
            ReceiverAccountId = @event.ReceiverAccountId;
            PayerAccountId = @event.PayerAccountId;
            PurchaseDateTime = @event.PurchaseDateTime;
            PlanId = @event.PlanId;
            PaymentId = @event.PaymentId;
            Amount = @event.Amount;
            Active = false;
            Days = @event.Days;
        }

    }
}
