using System;
using EzCommon.Commands;

namespace EzGym.Accounts.UpInsertAccountProfile
{
    public record UpInsertAccountProfileCommand(Guid AccountId, string Name, string JobDescription, string BioDescription) : ICommand;
}
