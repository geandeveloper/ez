using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Payments.Gateways;
using EzGym.Payments.Gateways.GerenciaNet.RequestPayment;

namespace EzGym.Payments.CreatePix
{
    public record CreatePaymentCommand(string PixKey, decimal Value, string Description) : ICommand;

    public record CreatePixCommandHandler : ICommandHandler<CreatePaymentCommand>
    {
        private readonly GatewayFactory _gatewayFactory;
        private readonly IGymEventStore _eventStore;

        public CreatePixCommandHandler(GatewayFactory gatewayFactory, IGymEventStore eventStore)
        {
            _gatewayFactory = gatewayFactory;
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            request = request with { PixKey = "d7d6bd52-0c8a-4b3e-a16c-1c8d267dd68d" };

            var payload = new RequestPaymentRequest
            {
                Calendario = new Calendario
                {
                    Expiracao = 3600
                },
                Chave = request.PixKey,
                SolicitacaoPagador = request.Description,
                Valor = new Valor
                {
                    Original = $"{request.Value:0.00}".Replace(",", ".")
                }
            };

            var paymentResponse = await _gatewayFactory.UseGerenciaNet(gateway => gateway.RequestPaymentAsync(payload));
            var qrCodeResponse = await _gatewayFactory.UseGerenciaNet(gateway => gateway.GenerateQRCodeAsync(paymentResponse.Loc.Id));

            var pix = new Pix(
                TxId: paymentResponse.Txid,
                QrCode: qrCodeResponse.Qrcode,
                QrCodeBase64Image: qrCodeResponse.ImagemQrcode);

            var payment = new Payment(request).CreatePix(pix);

            return await _eventStore.SaveAggregateAsync(payment);
        }
    }
}
