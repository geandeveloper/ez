using EzCommon.Commands;
using System;

namespace EzIdentity.Features.RefreshToken
{
    public record RefreshTokenCommand(Guid UserId, string RefreshToken) : ICommand;

}
