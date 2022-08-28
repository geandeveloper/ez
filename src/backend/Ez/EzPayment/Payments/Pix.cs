namespace EzPayment.Payments
{
    public record Pix(string QrCode, string QrCodeBase64Image);
    public record CreditCard(string ClientSecretKey);
}
