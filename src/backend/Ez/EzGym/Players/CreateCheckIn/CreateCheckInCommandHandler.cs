using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;

namespace EzGym.Players.CreateCheckIn;

public class CreateCheckInCommandHandler : ICommandHandler<CreateCheckInCommand>
{
    private readonly IGymRepository _repository;

    public CreateCheckInCommandHandler(IGymRepository repository)
    {
        _repository = repository;
    }

    public Task<EventStream> Handle(CreateCheckInCommand request, CancellationToken cancellationToken)
    {
        var checkIn = new CheckIn(request.PlayerId, request.GymAccountId, request.MemberShipId);

        return _repository.SaveAggregateAsync(checkIn);
    }
}
