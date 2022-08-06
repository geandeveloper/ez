using EzCommon.Commands;
using System;

namespace EzGym.Features.Accounts.UpInsertAccountProfile
{
    public record UpInsertAccountProfileCommand(Guid AccountId, string Name, string JobDescription, string BioDescription) : ICommand;
}
