using System;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzPayment.Infra.Repository;
using EzPayment.PaymentAccounts;

namespace EzPayment.Payments.CreatePayment
{
    public class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly CreatePaymentService _createPaymentService;

        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, CreatePaymentService createPaymentService)
        {
            _paymentRepository = paymentRepository;
            _createPaymentService = createPaymentService;
        }

        public async Task<EventStream> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment(request);
            var paymentAccount =
                await _paymentRepository.QueryAsync<PaymentAccount>(p => p.Id == request.DestinationPaymentAccountId);

            switch (request.PaymentMethod)
            {
                case PaymentMethodEnum.CreditCard:

                    var creditCardInfo = await _createPaymentService.CreateCardIntegrationPaymentAsync(request.Amount, request.Description, paymentAccount.IntegrationInfo.Id);
                    payment.PayWithCreditCard(creditCardInfo);

                    break;

                case PaymentMethodEnum.Pix:

                    var pixInfo = await _createPaymentService.CreatePixIntegrationPaymentAsync(request.Amount, request.Description);
                    payment.PayWithPix(pixInfo);

                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            return await _paymentRepository.SaveAggregateAsync(payment);
        }
    }
}
