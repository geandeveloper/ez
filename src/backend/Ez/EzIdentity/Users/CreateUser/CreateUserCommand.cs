using EzCommon.Commands;

namespace EzIdentity.Users.CreateUser
{
    public record CreateUserCommand(string Name, string UserName, string Email, string Password) : ICommand;
}
