using System.Threading.Tasks;
using EzPayment.Integrations.Gateways;
using EzPayment.Integrations.Gateways.GerenciaNet.RequestPayment;
using Microsoft.Extensions.Options;
using Stripe;

namespace EzPayment.Payments.CreatePayment
{
    public class CreatePaymentService
    {
        private readonly IOptions<EzPaymentSettings> _settings;
        private readonly GatewayFactory _gatewayFactory;

        public CreatePaymentService(IOptions<EzPaymentSettings> settings, GatewayFactory gatewayFactory)
        {
            _settings = settings;
            _gatewayFactory = gatewayFactory;
        }

        public async Task<PixInfo> CreatePixIntegrationPaymentAsync(long amount, string description)
        {
            var pixPayload = new RequestPaymentRequest
            {
                Calendario = new Calendario
                {
                    Expiracao = 3600
                },
                Chave = _settings.Value.GerenciaNet.PixKey,
                SolicitacaoPagador = description,
                Valor = new Valor
                {
                    Original = $"{amount:0.00}".Replace(",", ".")
                }
            };

            var pixPayment = await _gatewayFactory.UseGerenciaNet(gateway => gateway.RequestPaymentAsync(pixPayload));
            var qrCode = await _gatewayFactory.UseGerenciaNet(gateway => gateway.GenerateQrCodeAsync(pixPayment.Loc.Id));

            return new PixInfo(pixPayment.Txid, qrCode.Qrcode, qrCode.ImagemQrcode);
        }

        public Task<CreditCardInfo> CreateCardIntegrationPaymentAsync(long amount, string description)
        {

            var cardPayload = new PaymentIntentCreateOptions
            {
                Amount = amount * 100,
                Currency = "brl",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
                Description = description
            };

            var cardPayment = _gatewayFactory.UseStripePayment(gateway => gateway.CreatePaymentIntent(cardPayload));

            return Task.FromResult(new CreditCardInfo(cardPayment.Id, cardPayment.ClientSecret));
        }
    }
}
