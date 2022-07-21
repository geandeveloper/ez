using EzCommon.CommandHandlers;
using EzCommon.Infra.Security;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Models;
using EzIdentity.Services;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.Features.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand>
{

    private readonly TokenService _tokenService;
    private readonly IEventStore _eventStore;

    public LoginCommandHandler(
        TokenService tokenService,
        IEventStore eventStore
        )
    {
        _tokenService = tokenService;
        _eventStore = eventStore;
    }

    public async Task<EventStream> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = _eventStore.GetSnapShot<User>(user => user.Email == request.Email && user.Password == CryptographyService.CreateHash(request.Password));

        var accessToken = _tokenService.GenerateAccessToken(() => new Claim[]
            {
                    new Claim(nameof(User.Id), user.Id.ToString()),
                    new Claim(ClaimTypes.Email, request.Email),
            });

        var refreshToken = _tokenService.GenereateRefreshToken();

        user.SuccessLogin(accessToken, refreshToken);

        return await _eventStore.SaveAsync(user);
    }
}
