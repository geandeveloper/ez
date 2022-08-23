namespace EzGym.Payments
{
    public record PixData(string QrCode, string QrCodeBase64Image);
    public record PaymentReceipt(PaymentMethodEnum PaymentMethod, string TxId, string Name, PaymentStatusEnum Status, decimal Value, string Description, PixData PixData);
}
