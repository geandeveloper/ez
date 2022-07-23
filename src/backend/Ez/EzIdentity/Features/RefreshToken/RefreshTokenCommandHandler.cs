using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Extensions;
using EzIdentity.Models;
using EzIdentity.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.Features.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand>
{
    private readonly IEventStore _eventStore;

    public RefreshTokenCommandHandler(
        IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<EventStream> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _eventStore.GetSnapShot<User>(user => user.RefreshToken.Value == request.RefreshToken);

        if (user == null)
            throw new Exception("Invalid refresh token value");

        user.RenewToken();

        return await _eventStore.SaveAsync(user);
    }
}

