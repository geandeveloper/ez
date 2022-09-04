using System;
using EzCommon.Events;

namespace EzGym.Events.Gym;

public record GymMemberShipPaidEvent(string Id, DateTime PaymentDateTime) : Event;
public record GymMemberShipCreatedEvent(string Id, string ReceiverAccountId, string PayerAccountId, DateTime PurchaseDateTime, string PlanId, string PaymentId, long Amount, long ApplicationFeeAmount, int Days) : Event;