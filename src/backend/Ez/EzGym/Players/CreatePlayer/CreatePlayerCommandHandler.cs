using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;

namespace EzGym.Players.CreatePlayer;

public class CreatePlayerCommandHandler : ICommandHandler<CreatePlayerCommand>
{
    private readonly IGymRepository _repository;

    public CreatePlayerCommandHandler(IGymRepository repository)
    {
        _repository = repository;
    }

    public Task<EventStream> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = new Player(request.AccountId);

        return _repository.SaveAggregateAsync(player);
    }
}