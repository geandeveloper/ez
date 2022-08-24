using EzCommon.Commands;

namespace EzGym.Accounts.CreateAccount
{
    public record CreateAccountCommand(string UserId, string AccountName, AccountTypeEnum AccountType, bool IsDefault) : ICommand;
}
