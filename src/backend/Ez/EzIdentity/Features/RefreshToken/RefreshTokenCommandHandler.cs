using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Infra.Storage;
using EzIdentity.Models;
using EzIdentity.SnapShots;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.Features.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand>
{
    private readonly IQueryStorage _queryStorage;
    private readonly IEventStore _eventStore;

    public RefreshTokenCommandHandler(
        IIdentityEventStore eventStore,
        IIdentityQueryStore queryStorage
        )
    {
        _eventStore = eventStore;
        _queryStorage = queryStorage;
    }

    public async Task<EventStream> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var snapShot = _queryStorage.GetSnapShot<UserSnapShot>(user => user.RefreshToken.Value == request.RefreshToken);
        var user = User.RestoreSnapShot(snapShot);

        if (user == null)
            throw new Exception("Invalid refresh token value");

        user.RenewToken();

        return await _eventStore.SaveAsync<User, UserSnapShot>(user);
    }
}

