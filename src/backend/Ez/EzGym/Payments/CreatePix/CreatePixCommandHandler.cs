using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;
using EzGym.Payments.Gateways;
using EzGym.Payments.Gateways.GerenciaNet.RequestPayment;

namespace EzGym.Payments.CreatePix
{
    public record CreatePaymentCommand(string PixKey, decimal Value, string Description) : ICommand;

    public record CreatePixCommandHandler : ICommandHandler<CreatePaymentCommand>
    {
        private readonly GatewayFactory _gatewayFactory;
        private readonly IGymRepository _repository;

        public CreatePixCommandHandler(GatewayFactory gatewayFactory, IGymRepository repository)
        {
            _gatewayFactory = gatewayFactory;
            _repository = repository;
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

            //var paymentResponse = await _gatewayFactory.UseGerenciaNet(gateway => gateway.RequestPaymentAsync(payload));
            //var qrCodeResponse = await _gatewayFactory.UseGerenciaNet(gateway => gateway.GenerateQRCodeAsync(paymentResponse.Loc.Id));

            var pix = new Pix(
                TxId: payload.Chave,
                QrCode: payload.Chave,
                QrCodeBase64Image: payload.Chave);

            var payment = new Payment(request).CreatePix(pix);

            return await _repository.SaveAggregateAsync(payment);
        }
    }
}
