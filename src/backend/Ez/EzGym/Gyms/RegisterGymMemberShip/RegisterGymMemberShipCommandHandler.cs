using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Infra.Repository;
using EzPayment.Events.Payments;
using EzPayment.Payments.CreatePayment;
using EzPayment.Payments.CreatePix;
using CreatePaymentCommand = EzPayment.Payments.CreatePayment.CreatePaymentCommand;

namespace EzGym.Gyms.RegisterGymMemberShip;

public record RegisterGymMemberShipCommand(string PayerAccountId, string GymId, string PlanId) : ICommand;

public class RegisterGymMemberShipCommandHandler : ICommandHandler<RegisterGymMemberShipCommand>
{
    private readonly CreatePixCommandHandler _createPix;
    private readonly CreatePaymentCommandHandler _paymentCheckout;
    private readonly IGymRepository _repository;

    public RegisterGymMemberShipCommandHandler(
        CreatePixCommandHandler createPix,
        IGymRepository repository, CreatePaymentCommandHandler paymentCheckout)
    {
        _createPix = createPix;
        _repository = repository;
        _paymentCheckout = paymentCheckout;
    }

    public async Task<EventStream> Handle(RegisterGymMemberShipCommand request, CancellationToken cancellationToken)
    {
        var payerAccount = await _repository.LoadAggregateAsync<Account>(request.PayerAccountId)!;

        var receiverGym = await _repository.QueryAsync<Gym>(g => g.Id == request.GymId);
        var plan = receiverGym.GymPlans.First(p => p.Id == request.PlanId);

        var paymentStream = await _paymentCheckout
            .Handle(new CreatePaymentCommand(plan.Amount, $"Plano de {plan.Days} dias"), cancellationToken);

        var payment = paymentStream.GetEvent<PaymentCreatedEvent>();

        var membership = new GymMemberShip(
            receiverAccountId: receiverGym.AccountId,
            payerAccountId: payerAccount.Id,
            plan.Id,
            payment.Id,
            plan.Amount,
            plan.Days);

        return await _repository.SaveAggregateAsync(membership);
    }
}
