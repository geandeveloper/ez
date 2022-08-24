using System;
using EzCommon.Commands;

namespace EzGym.Accounts.UpInsertAccountProfile
{
    public record UpInsertAccountProfileCommand(string AccountId, string Name, string JobDescription, string BioDescription) : ICommand;
}
