using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Features.RevokeToken;
using EzIdentity.Models;
using System.Threading;
using System.Threading.Tasks;

public class RevokeTokenCommandHandler : ICommandHandler<RevokeTokenCommand>
{
    private readonly IEventStore _eventStore;

    public RevokeTokenCommandHandler(
        IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<EventStream> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _eventStore.GetSnapShot<User>(user => user.RefreshToken.Value == request.RefreshToken);

        user.RevokeToken(request.RefreshToken);

        return await _eventStore.SaveAsync(user);
    }
}

