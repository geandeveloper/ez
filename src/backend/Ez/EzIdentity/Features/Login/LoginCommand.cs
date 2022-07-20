using EzCommon.Commands;

namespace EzIdentity.Features.Login;

public record LoginCommand(string Email, string Password) : ICommand;

