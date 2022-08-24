using System;
using System.IO;
using EzCommon.Commands;

namespace EzGym.Accounts.ChangeAvatar
{
    public record ChangeAvatarCommand(string UserId, string AccountId, string FileName, MemoryStream AvatarStream) : ICommand;
}
