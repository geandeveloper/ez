using EzCommon.Commands;

namespace EzIdentity.Features.CreateUser
{
    public record CreateUserCommand(string Name, string Email, string Password) : ICommand;
}
