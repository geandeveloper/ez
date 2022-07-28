using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.Features.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand>
{
    private readonly IQueryStorage _queryStorage;
    private readonly IEventStore _eventStore;

    public RefreshTokenCommandHandler(
        IEventStore eventStore, 
        IQueryStorage queryStorage
        )
    {
        _eventStore = eventStore;
        _queryStorage = queryStorage;
    }

    public async Task<EventStream> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _queryStorage.GetSnapShot<User>(user => user.RefreshToken.Value == request.RefreshToken);

        if (user == null)
            throw new Exception("Invalid refresh token value");

        user.RenewToken();

        return await _eventStore.SaveAsync(user);
    }
}

