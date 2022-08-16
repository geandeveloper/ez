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

    public RevokeTokenCommandHandler(IIdentityEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<EventStream> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _eventStore.QueryAsync<User>(user => user.RefreshToken.Value == request.RefreshToken);

        user.RevokeToken(request.RefreshToken);

        return await _eventStore.SaveAggregateAsync(user);
    }
}

