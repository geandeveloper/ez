using System;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzPayment.Infra.Repository;
using EzPayment.PaymentAccounts;
using Microsoft.Extensions.Options;

namespace EzPayment.Payments.CreatePayment
{
    public class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly CreatePaymentService _createPaymentService;
        private readonly IOptions<EzPaymentSettings> _settings;

        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, CreatePaymentService createPaymentService, IOptions<EzPaymentSettings> settings)
        {
            _paymentRepository = paymentRepository;
            _createPaymentService = createPaymentService;
            _settings = settings;
        }

        public async Task<EventStream> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var paymentAmount = request.Amount;
            var paymentFeeAmount = Convert.ToInt64(paymentAmount * _settings.Value.StripePayments.EzPaymentFeeAmount);

            var payment = new Payment(request, paymentFeeAmount);
            var paymentAccount = await _paymentRepository
                .QueryAsync<PaymentAccount>(p => p.Id == request.DestinationPaymentAccountId);

            switch (request.PaymentMethod)
            {
                case PaymentMethodEnum.CreditCard:

                    var creditCardInfo = await _createPaymentService.CreateCardIntegrationPaymentAsync(request.Amount * 100, paymentFeeAmount * 100, request.Description, paymentAccount.IntegrationInfo.Id);
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
