using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Infra.Storage;
using EzGym.Payments.Gateways;
using EzGym.Payments.Gateways.GerenciaNet.RequestPayment;

namespace EzGym.Payments.CreatePayment
{
    public record CreatePaymentCommand(PaymentMethodEnum PaymentMethod, decimal Value, Guid PayerAccountId, Guid ReceiverAccountId, string Description) : ICommand;

    public record CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand>
    {
        private readonly GatewayFactory _gatewayFactory;
        private readonly IGymEventStore _eventStore;

        public CreatePaymentCommandHandler(GatewayFactory gatewayFactory, IGymEventStore eventStore)
        {
            _gatewayFactory = gatewayFactory;
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payerAccount = await _eventStore.LoadAggregateAsync<Account>(request.PayerAccountId);
            var receiverAccount = await _eventStore.LoadAggregateAsync<Account>(request.ReceiverAccountId);

            var payload = new RequestPaymentRequest
            {
                Calendario = new Calendario
                {
                    Expiracao = 3600
                },
                Chave = "d7d6bd52-0c8a-4b3e-a16c-1c8d267dd68d",
                SolicitacaoPagador = request.Description,
                Valor = new Valor
                {
                    Original = $"{request.Value:0.00}".Replace(",", ".")
                }
            };

            var paymentResponse = await _gatewayFactory.UseGerenciaNet(gateway => gateway.RequestPaymentAsync(payload));
            var qrCodeResponse = await _gatewayFactory.UseGerenciaNet(gateway => gateway.GenerateQRCodeAsync(paymentResponse.Loc.Id));

            var paymentReceipt = new PaymentReceipt(
                TxId: paymentResponse.Txid,
                PaymentMethod: request.PaymentMethod,
                Name: "Pagamento Via Pix",
                Status: PaymentStatusEnum.Pending,
                Value: request.Value,
                Description: $"Pagamento realizado para - {receiverAccount.AccountName}",
                PixData: new PixData(qrCodeResponse.Qrcode, qrCodeResponse.ImagemQrcode));

            var payment = new Payment(request, paymentReceipt);

            return await _eventStore.SaveAggregateAsync(payment);
        }
    }
}
