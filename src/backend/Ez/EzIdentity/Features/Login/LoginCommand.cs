using EzCommon.Commands;

namespace EzIdentity.Features.Login;

public record LoginCommand(string UserName, string Password) : ICommand;

