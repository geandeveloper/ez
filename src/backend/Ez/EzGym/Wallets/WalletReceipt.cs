using EzGym.Payments;

namespace EzGym.Wallets
{
    public record WalletReceipt(PaymentMethodEnum PaymentMethod, string Name, PaymentStatusEnum Status, decimal Value, string Description);
}
