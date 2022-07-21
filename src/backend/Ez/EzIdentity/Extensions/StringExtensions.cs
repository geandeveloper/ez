using System;

namespace EzIdentity.Extensions;

public static class StringExtensions
{
    public static Guid ToGuid(this string @string) => Guid.Parse(@string);
}
