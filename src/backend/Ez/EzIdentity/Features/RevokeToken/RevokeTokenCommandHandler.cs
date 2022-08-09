using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Features.RevokeToken;
using EzIdentity.Infra.Storage;
using EzIdentity.Models;
using EzIdentity.SnapShots;
using System.Threading;
using System.Threading.Tasks;

public class RevokeTokenCommandHandler : ICommandHandler<RevokeTokenCommand>
{
    private readonly IEventStore _eventStore;
    private readonly IQueryStorage _queryStorage;

    public RevokeTokenCommandHandler(
        IIdentityEventStore eventStore,
        IIdentityQueryStore queryStorage
        )
    {
        _eventStore = eventStore;
        _queryStorage = queryStorage;
    }

    public async Task<EventStream> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var snapShot = _queryStorage.QueryOne<UserSnapShot>(user => user.RefreshToken.Value == request.RefreshToken);
        var user = User.RestoreSnapShot(snapShot);

        user.RevokeToken(request.RefreshToken);

        return await _eventStore.SaveAsync<User, UserSnapShot>(user);
    }
}

