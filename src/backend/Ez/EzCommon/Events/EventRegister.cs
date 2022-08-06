using System;
using System.Collections.Generic;

namespace EzCommon.Events
{
    public class EventRegister : IEventRegister
    {
        private readonly IDictionary<string, Type> _eventRegisters;

        private EventRegister()
        {
            _eventRegisters = new Dictionary<string, Type>();
        }

        public static IEventRegister Factory()
        {
            return new EventRegister();
        }

        public Type GetEventType(string eventName)
        {
            return _eventRegisters[eventName];
        }

        public IEventRegister Register<TEvent>()
        {
            _eventRegisters.Add(typeof(TEvent).Name, typeof(TEvent));
            return this;
        }
    }
}
