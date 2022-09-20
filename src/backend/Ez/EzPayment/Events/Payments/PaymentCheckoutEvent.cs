using EzCommon.Events;

namespace EzPayment.Events.Payments;

public record PaymentCheckoutEvent(string PaymentId, string SessionId) : Event;
