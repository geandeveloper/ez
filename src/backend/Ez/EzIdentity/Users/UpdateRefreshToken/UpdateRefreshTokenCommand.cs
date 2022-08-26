using EzCommon.Commands;

namespace EzIdentity.Users.UpdateRefreshToken
{
    public record UpdateRefreshTokenCommand(string RefreshToken) : ICommand;

}
