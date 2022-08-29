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
using EzPayment.Payments;
using EzPayment.Payments.CreatePayment;
using CreatePaymentCommand = EzPayment.Payments.CreatePayment.CreatePaymentCommand;

namespace EzGym.Gyms.RegisterGymMemberShip;

public record RegisterGymMemberShipCommand(string PayerAccountId, string GymId, string PlanId, PaymentMethodEnum PaymentMethod) : ICommand;

public class RegisterGymMemberShipCommandHandler : ICommandHandler<RegisterGymMemberShipCommand>
{
    private readonly CreatePaymentCommandHandler _createPayment;
    private readonly IGymRepository _repository;

    public RegisterGymMemberShipCommandHandler(IGymRepository repository, CreatePaymentCommandHandler createPayment)
    {
        _repository = repository;
        _createPayment = createPayment;
    }

    public async Task<EventStream> Handle(RegisterGymMemberShipCommand request, CancellationToken cancellationToken)
    {
        var payerAccount = await _repository.LoadAggregateAsync<Account>(request.PayerAccountId)!;

        var receiverGym = await _repository.QueryAsync<Gym>(g => g.Id == request.GymId);
        var plan = receiverGym.GymPlans.First(p => p.Id == request.PlanId);

        var paymentStream = await _createPayment
            .Handle(new CreatePaymentCommand(plan.Amount, $"Plano de {plan.Days} dias", request.PaymentMethod), cancellationToken);

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
