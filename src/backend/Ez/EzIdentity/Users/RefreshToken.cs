using System;

namespace EzIdentity.Users
{
    public record RefreshToken(string Value, DateTime Created, DateTime Expires);
}
