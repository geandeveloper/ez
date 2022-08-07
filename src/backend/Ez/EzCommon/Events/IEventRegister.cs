using System;

namespace EzCommon.Events
{
    public interface IEventRegister
    {
        IEventRegister Register<TEvent>();
        Type GetRegisterType(string registerName);
    }
}
