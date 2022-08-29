using EzCommon.Events;
using EzPayment.Payments;

namespace EzPayment.Events.Payments;

public record PixPaymentCreatedEvent(string PaymentId, PixInfo PixInfo) : Event;
public record CreditCardPaymentCreatedEvent(string PaymentId, CreditCardInfo CardInfo) : Event;