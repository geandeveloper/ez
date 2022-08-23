using EzCommon.Events;
using EzGym.Payments;

namespace EzGym.Wallets.Events
{
    public record IncommingReceivedEvent(PaymentReceipt Receipt) : Event;
}
