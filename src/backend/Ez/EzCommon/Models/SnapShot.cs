using System;

namespace EzCommon.Models
{
    public abstract class SnapShot
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
