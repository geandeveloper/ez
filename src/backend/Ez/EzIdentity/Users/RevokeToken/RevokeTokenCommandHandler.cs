using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzIdentity.Infra.Repository;

namespace EzIdentity.Users.RevokeToken;

public class RevokeTokenCommandHandler : ICommandHandler<RevokeTokenCommand>
{
    private readonly IIdentityRepository _repository;

    public RevokeTokenCommandHandler(IIdentityRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventStream> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.QueryAsync<User>(user => user.RefreshToken.Value == request.RefreshToken);

        user.RevokeToken(request.RefreshToken);

        return await _repository.SaveAggregateAsync(user);
    }
}