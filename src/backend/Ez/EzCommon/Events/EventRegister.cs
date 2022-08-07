using System;
using System.Collections.Generic;

namespace EzCommon.Events
{

    public class EventRegister : IEventRegister
    {

        private record SnapShotEventType(Type EventType, Type SnapShotType);

        private readonly Dictionary<string, Type> _eventRegisters;

        private EventRegister()
        {
            _eventRegisters = new Dictionary<string, Type>();
        }

        public static IEventRegister Factory()
        {
            return new EventRegister();
        }

        public Type GetRegisterType(string registerName)
        {
            return _eventRegisters[registerName];
        }

        public IEventRegister Register<TEvent>()
        {
            _eventRegisters.Add(typeof(TEvent).FullName, typeof(TEvent));
            return this;
        }
    }
}
