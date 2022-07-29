using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Features.RevokeToken;
using EzIdentity.Infra.Storage;
using EzIdentity.Models;
using System.Threading;
using System.Threading.Tasks;

public class RevokeTokenCommandHandler : ICommandHandler<RevokeTokenCommand>
{
    private readonly IEventStore _eventStore;
    private readonly IQueryStorage _queryStorage;

    public RevokeTokenCommandHandler(
        IIdentityEventStore eventStore,
        IIdentityQueryStorage queryStorage
        )
    {
        _eventStore = eventStore;
        _queryStorage = queryStorage;
    }

    public async Task<EventStream> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _queryStorage.GetSnapShot<User>(user => user.RefreshToken.Value == request.RefreshToken);

        user.RevokeToken(request.RefreshToken);

        return await _eventStore.SaveAsync(user);
    }
}

