using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Infra.Security;
using EzCommon.Models;
using EzIdentity.Infra.Repository;
using EzIdentity.Services;
using Microsoft.Extensions.Options;

namespace EzIdentity.Users.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand>
{
    private readonly IIdentityRepository _repository;
    private readonly IOptions<EzIdentitySettings> _settings;

    public LoginCommandHandler(IIdentityRepository repository, IOptions<EzIdentitySettings> settings)
    {
        _repository = repository;
        _settings = settings;
    }

    public async Task<EventStream> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.QueryAsync<User>(user => user.UserName == request.UserName && user.Password == CryptographyService.CreateHash(request.Password));

        var accessToken = TokenService.GenerateAccessToken(() => new Claim[]
        {
                    new(nameof(user.Id), user.Id),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.NameIdentifier, user.UserName),
                    new(ClaimTypes.Name, user.Name ?? user.UserName)

        }, _settings.Value.TokenSecurityKey, _settings.Value.EzIdentityUrl);

        var refreshToken = TokenService.GenerateRefreshToken();

        user.Login(accessToken, refreshToken);

        return await _repository.SaveAggregateAsync(user);
    }
}
