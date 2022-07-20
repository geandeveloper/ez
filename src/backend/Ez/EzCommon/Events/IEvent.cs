using System;
using MediatR;

namespace EzCommon.Events
{
    public interface IEvent : INotification
    {
        int Version { get; set; }
        DateTime TimeStamp { get; }
    }
}