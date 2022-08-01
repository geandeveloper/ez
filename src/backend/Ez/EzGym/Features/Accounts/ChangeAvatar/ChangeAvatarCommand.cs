using EzCommon.Commands;
using System;
using System.IO;

namespace EzGym.Features.Accounts.ChangeAvatar
{
    public record ChangeAvatarCommand(Guid UserId, Guid AccountId, string FileName, MemoryStream AvatarStream) : ICommand;
}
