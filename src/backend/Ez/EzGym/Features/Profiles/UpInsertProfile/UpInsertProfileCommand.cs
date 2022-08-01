using EzCommon.Commands;
using System;

namespace EzGym.Features.Profiles.UpInsertProfile
{
    public record UpInsertProfileCommand(Guid AccountId, string Name, string JobDescription, string BioDescription) : ICommand;
}
