using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;

namespace EzGym.Players.CreatePlayer;

public class CreatePlayerCommandHandler : ICommandHandler<CreatePlayerCommand>
{
    public Task<EventStream> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
    }
}