using EzCommon.Commands;
using System;

namespace EzIdentity.Features.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : ICommand;

}
