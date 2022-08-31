using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzPayment.Infra.Repository;

namespace EzPayment.PaymentAccounts.CreatePaymentAccountSetupLink
{
    public record CreatePaymentAccountSetupLinkCommand(string PaymentAccountId, string RefreshUrl, string ReturnUrl) : ICommand;

    public class CreatePaymentAccountSetupLinkCommandHandler : ICommandHandler<CreatePaymentAccountSetupLinkCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly CreatePaymentAccountSetupLinkService _accountSetupLinkService;

        public CreatePaymentAccountSetupLinkCommandHandler(IPaymentRepository paymentRepository, CreatePaymentAccountSetupLinkService accountSetupLinkService)
        {
            _paymentRepository = paymentRepository;
            _accountSetupLinkService = accountSetupLinkService;
        }

        public async Task<EventStream> Handle(CreatePaymentAccountSetupLinkCommand request, CancellationToken cancellationToken)
        {
            var paymentAccount = await _paymentRepository.LoadAggregateAsync<PaymentAccount>(request.PaymentAccountId);
            var paymentAccountLink = await
                _accountSetupLinkService.CreateAccountOnBoardingLinkIntegrationAsync(paymentAccount.IntegrationInfo.Id,
                    request.RefreshUrl, request.ReturnUrl);
            paymentAccount.UpdateOnBoardingLink(paymentAccountLink);

            return await _paymentRepository.SaveAggregateAsync(paymentAccount);
        }
    }
}
