using EzCommon.Events;
using EzPayment.Payments;

namespace EzPayment.Events.Payments;

public record PaymentCheckoutEvent(string PaymentId, string SessionId) : Event;