using System;

namespace EzCommon.Models
{
    public abstract class SnapShot
    {
        public string Id { get; set; }
        public int Version { get; set; }
    }
}
