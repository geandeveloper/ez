using EzCommon.Commands;
using System;
using System.IO;

namespace EzGym.Features.Profiles.ChangeAvatar
{
    public record ChangeAvatarCommand(Guid UserId, Guid AccountId, MemoryStream AvatarStream) : ICommand;
}
