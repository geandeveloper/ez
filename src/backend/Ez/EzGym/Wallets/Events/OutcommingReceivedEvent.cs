using EzCommon.Events;
using EzGym.Payments;

namespace EzGym.Wallets.Events
{
    public record OutcommingReceivedEvent(PaymentReceipt Receipt) : Event;
}
