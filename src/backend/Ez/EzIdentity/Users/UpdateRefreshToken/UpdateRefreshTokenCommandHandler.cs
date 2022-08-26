using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzIdentity.Infra.Repository;
using EzIdentity.Services;
using Microsoft.Extensions.Options;

namespace EzIdentity.Users.UpdateRefreshToken;

public class UpdateRefreshTokenCommandHandler : ICommandHandler<UpdateRefreshTokenCommand>
{
    private readonly IIdentityRepository _repository;
    private readonly IOptions<EzIdentitySettings> _settings;

    public UpdateRefreshTokenCommandHandler(
        IIdentityRepository repository,
        IOptions<EzIdentitySettings> settings)
    {
        _repository = repository;
        _settings = settings;
    }

    public async Task<EventStream> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = _repository.QueryOne<User>(user => user.RefreshToken.Value == request.RefreshToken);

        if (user == null)
            throw new Exception("Invalid refresh token value");

        if (user.RefreshToken.Expires <= DateTime.UtcNow)
            throw new Exception("Refresh token already expired, please login again");

        var accessToken = TokenService.GenerateAccessToken(() => new Claim[]
            {
                    new(nameof(user.Id), user.Id),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.NameIdentifier, user.UserName),
                    new(ClaimTypes.Name, user.Name ?? user.UserName)

            }, _settings.Value.TokenSecurityKey, _settings.Value.EzIdentityUrl);

        var refreshToken = TokenService.GenerateRefreshToken();


        user.RenewToken(accessToken, refreshToken);

        return await _repository.SaveAggregateAsync(user);
    }
}

