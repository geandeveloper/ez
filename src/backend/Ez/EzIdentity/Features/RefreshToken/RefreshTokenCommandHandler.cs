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
    private readonly TokenService _tokenService;
    private readonly IEventStore _eventStore;

    public RefreshTokenCommandHandler(
        TokenService tokenService,
        IEventStore eventStore)
    {
        _tokenService = tokenService;
        _eventStore = eventStore;
    }

    public async Task<EventStream> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _eventStore.GetSnapShot<User>(user => user.RefreshToken.Value == request.RefreshToken);

        if (user == null)
            throw new Exception("Invalid refresh token value");

        if (user.RefreshToken.Expires <= DateTime.UtcNow)
            throw new Exception("Refresh token already expired, please login again");

        var accessToken = _tokenService.GenerateAccessToken(() => new Claim[]
            {
                    new Claim(nameof(User.Id), user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
            });

        var refreshToken = _tokenService.GenereateRefreshToken();
        user.SuccessRefreshToken(accessToken, refreshToken);

        return await _eventStore.SaveAsync(user);
    }
}

