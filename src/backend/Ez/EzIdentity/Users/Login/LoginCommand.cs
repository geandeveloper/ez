using EzCommon.Commands;

namespace EzIdentity.Users.Login;

public record LoginCommand(string UserName, string Password) : ICommand;

