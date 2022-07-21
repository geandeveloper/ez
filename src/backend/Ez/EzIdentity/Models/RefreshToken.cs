using System;

namespace EzIdentity.Models
{
    public record RefreshToken(string Value, DateTime Created, DateTime Expires);
}
