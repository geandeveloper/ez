using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Gyms.Users.CreateGymUser;
using EzGym.Infra.Repository;
using EzPayment.Events.Payments;
using EzPayment.Payments.CreatePayment;
using EzPayment.Payments.CreatePix;
using CreatePaymentCommand = EzPayment.Payments.CreatePayment.CreatePaymentCommand;

namespace EzGym.Gyms.Users.RegisterGymMemberShip;

public record RegisterGymMemberShipCommand(string PayerAccountId, string GymId, string PlanId) : ICommand;

public class RegisterGymMemberShipCommandHandler : ICommandHandler<RegisterGymMemberShipCommand>
{
    private readonly CreateGymUserCommandHandler _createGymUserCommandHandler;
    private readonly CreatePixCommandHandler _createPix;
    private readonly CreatePaymentCommandHandler _paymentCheckout;
    private readonly IGymRepository _repository;

    public RegisterGymMemberShipCommandHandler(
        CreateGymUserCommandHandler createGymUserCommandHandler,
        CreatePixCommandHandler createPix,
        IGymRepository repository, CreatePaymentCommandHandler paymentCheckout)
    {
        _createGymUserCommandHandler = createGymUserCommandHandler;
        _createPix = createPix;
        _repository = repository;
        _paymentCheckout = paymentCheckout;
    }

    public async Task<EventStream> Handle(RegisterGymMemberShipCommand request, CancellationToken cancellationToken)
    {
        var payerAccount = await _repository.LoadAggregateAsync<Account>(request.PayerAccountId)!;
        var payerGymUser = await _repository.QueryAsync<GymUser>(g => g.AccountId == request.PayerAccountId);

        if (payerGymUser == null)
        {
            var eventStream = await _createGymUserCommandHandler.Handle(
                new CreateGymUserCommand(request.PayerAccountId, payerAccount.Profile?.Name, payerAccount.AccountName),
                cancellationToken);

            payerGymUser = await _repository.LoadAggregateAsync<GymUser>(eventStream.Id);
        }

        payerGymUser.AssociateGymUserWithAccount(payerAccount.Id);


        var receiverGym = await _repository.QueryAsync<Gym>(g => g.Id == request.GymId);
        var plan = receiverGym.GymPlans.First(p => p.Id == request.PlanId);

        var paymentStream = await _paymentCheckout 
            .Handle(new CreatePaymentCommand(plan.Amount, $"Plano de {plan.Days} dias"), cancellationToken);

        var payment = paymentStream.GetEvent<PaymentCreatedEvent>();

        var membership = GymMemberShip.CreatePendingMemberShip(
            Guid.NewGuid().ToString(),
            receiverAccountId: receiverGym.AccountId,
            payerAccountId: request.PayerAccountId,
            plan.Id,
            payment.Id,
            plan.Amount,
            plan.Days);

        payerGymUser.AddMemberShip(membership);

        return await _repository.SaveAggregateAsync(payerGymUser);
    }
}
