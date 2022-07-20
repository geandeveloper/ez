using EzCommon.CommandHandlers;
using EzCommon.Events;
using EzCommon.Models;
using EzIdentity.Services;
using System.Security.Claims;

namespace EzIdentity.Features.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand>
{

    private readonly TokenService _tokenService;

    public LoginCommandHandler(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public Task<EventStream> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _tokenService.GenerateToken(() =>
        {
            return new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Email, request.Email)
            });
        });

        return Task.FromResult(new EventStream(new EventStreamId(), new List<IEvent>()));
    }
}
