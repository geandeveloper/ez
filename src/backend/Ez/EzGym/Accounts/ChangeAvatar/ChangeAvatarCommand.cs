using System;
using System.IO;
using EzCommon.Commands;

namespace EzGym.Accounts.ChangeAvatar
{
    public record ChangeAvatarCommand(Guid UserId, Guid AccountId, string FileName, MemoryStream AvatarStream) : ICommand;
}
