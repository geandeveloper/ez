namespace EzPayment.Payments
{
    public record Pix(string TxId, string QrCode, string QrCodeBase64Image);
}
