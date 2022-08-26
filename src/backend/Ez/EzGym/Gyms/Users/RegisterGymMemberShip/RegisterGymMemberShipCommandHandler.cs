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
using EzGym.Infra.Storage;
using EzGym.Payments.CreatePix;
using EzGym.Payments.Events;

namespace EzGym.Gyms.Users.RegisterGymMemberShip;

public record RegisterGymMemberShipCommand(string GymId, string AccountId, string PlanId) : ICommand;

public class RegisterGymMemberShipCommandHandler : ICommandHandler<RegisterGymMemberShipCommand>
{
    private readonly CreateGymUserCommandHandler _createGymUserCommandHandler;
    private readonly CreatePixCommandHandler _createPixCommandHandler;
    private readonly IGymRepository _repository;

    public RegisterGymMemberShipCommandHandler(
        CreateGymUserCommandHandler createGymUserCommandHandler,
        IGymRepository repository,
        CreatePixCommandHandler createPixCommandHandler)
    {
        _createGymUserCommandHandler = createGymUserCommandHandler;
        _repository = repository;
        _createPixCommandHandler = createPixCommandHandler;
    }

    public async Task<EventStream> Handle(RegisterGymMemberShipCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.LoadAggregateAsync<Account>(request.AccountId)!;

        var eventStream = await _createGymUserCommandHandler.Handle(
            new CreateGymUserCommand(request.GymId, account.Profile?.Name, account.AccountName),
            cancellationToken);

        var gymUser = await _repository.LoadAggregateAsync<GymUser>(eventStream.Id);
        gymUser.AssociateGymUserWithAccount(account.Id);


        var gym = await _repository.LoadAggregateAsync<Gym>(request.GymId);
        var plan = gym.GymPlans.First(p => p.Id == request.PlanId);
        var paymentStream = await _createPixCommandHandler
            .Handle(new CreatePaymentCommand("", plan.Price, $"Plano de {plan.Days} dias"), cancellationToken);

        var payment = paymentStream.GetEvent<PaymentCreatedEvent>();

        var membership = GymMemberShip.CreatePendingMemberShip(
            Guid.NewGuid().ToString(),
            gymId: request.GymId,
            gymUserId: eventStream.Id,
            plan.Id,
            payment.Id,
            plan.Price,
            plan.Days);

        gymUser.AddMemberShip(membership);

        return await _repository.SaveAggregateAsync(gymUser);
    }
}
