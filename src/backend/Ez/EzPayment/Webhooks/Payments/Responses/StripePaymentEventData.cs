using Newtonsoft.Json;

namespace EzPayment.Webhooks.Payments.Responses
{
    public record StripePaymentEventData(
        string Id,

        [JsonProperty("client_secret")]
        string ClientSecret,

        [JsonProperty("amount_received")]
        long Amount
    );
}
