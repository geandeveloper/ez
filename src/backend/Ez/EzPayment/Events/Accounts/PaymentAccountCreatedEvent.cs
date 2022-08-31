using EzCommon.Events;

namespace EzPayment.Events.Accounts
{
    public record PaymentAccountCreatedEvent(string Id, string IntegrationId) : Event;
}
