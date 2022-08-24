using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Gyms.Users.CreateGymUser;
using EzGym.Infra.Storage;
using EzGym.Payments.CreatePix;
using EzGym.Payments.Events;

namespace EzGym.Gyms.Users.RegisterGymMemberShip;

public record RegisterGymMemberShipCommand(string GymId, string AccountId, string PlanId) : ICommand;

public class RegisterGymMemberShipCommandHandler : ICommandHandler<RegisterGymMemberShipCommand>
{
    private readonly CreateGymUserCommandHandler _createGymUserCommandHandler;
    private readonly CreatePixCommandHandler _createPixCommandHandler;
    private readonly IGymEventStore _eventStore;

    public RegisterGymMemberShipCommandHandler(
        CreateGymUserCommandHandler createGymUserCommandHandler,
        IGymEventStore eventStore,
        CreatePixCommandHandler createPixCommandHandler)
    {
        _createGymUserCommandHandler = createGymUserCommandHandler;
        _eventStore = eventStore;
        _createPixCommandHandler = createPixCommandHandler;
    }

    public async Task<EventStream> Handle(RegisterGymMemberShipCommand request, CancellationToken cancellationToken)
    {
        var account = await _eventStore.LoadAggregateAsync<Account>(request.AccountId)!;

        var eventStream = await _createGymUserCommandHandler.Handle(
            new CreateGymUserCommand(request.GymId, account.Profile?.Name, account.AccountName),
            cancellationToken);

        var gymUser = await _eventStore.LoadAggregateAsync<GymUser>(eventStream.Id);
        gymUser.AssociateGymUserWithAccount(account.Id);


        var gym = await _eventStore.LoadAggregateAsync<Gym>(request.GymId);
        var plan = gym.GymPlans.First(p => p.Id == request.PlanId);
        var paymentStream = await _createPixCommandHandler
            .Handle(new CreatePaymentCommand("", plan.Price, $"Plano de {plan.Days} dias"), cancellationToken);

        var payment = paymentStream.GetEvent<PaymentCreatedEvent>();

        var membership = GymMemberShip.CreatePendingMemberShip(
            Guid.NewGuid().ToString(),
            payment.Id,
            plan.Id,
            plan.Price,
            plan.Days);

        gymUser.AddMemberShip(membership);

        return await _eventStore.SaveAggregateAsync(gymUser);
    }
}
