using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzPayment.Infra.Repository;
using EzPayment.Integrations.Gateways;
using EzPayment.Integrations.Gateways.GerenciaNet.RequestPayment;
using Microsoft.Extensions.Options;

namespace EzPayment.Payments.CreatePix
{
    public record CreatePaymentCommand(decimal Value, string Description) : ICommand;

    public record CreatePixCommandHandler : ICommandHandler<CreatePaymentCommand>
    {
        private readonly GatewayFactory _gatewayFactory;
        private readonly IPaymentRepository _repository;
        private readonly IOptions<EzPaymentSettings> _settings;

        public CreatePixCommandHandler(GatewayFactory gatewayFactory, IPaymentRepository repository, IOptions<EzPaymentSettings> settings)
        {
            _gatewayFactory = gatewayFactory;
            _repository = repository;
            _settings = settings;
        }

        public async Task<EventStream> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payload = new RequestPaymentRequest
            {
                Calendario = new Calendario
                {
                    Expiracao = 3600
                },
                Chave = _settings.Value.GerenciaNet.PixKey,
                SolicitacaoPagador = request.Description,
                Valor = new Valor
                {
                    Original = $"{request.Value:0.00}".Replace(",", ".")
                }
            };

            var paymentResponse = await _gatewayFactory.UseGerenciaNet(gateway => gateway.RequestPaymentAsync(payload));
            var qrCodeResponse = await _gatewayFactory.UseGerenciaNet(gateway => gateway.GenerateQRCodeAsync(paymentResponse.Loc.Id));

            var pix = new Pix(
                QrCode: qrCodeResponse.Qrcode,
                QrCodeBase64Image: qrCodeResponse.ImagemQrcode);

            var payment = new Payment(paymentResponse.Txid, request).CreatePix(pix);

            return await _repository.SaveAggregateAsync(payment);
        }
    }
}
