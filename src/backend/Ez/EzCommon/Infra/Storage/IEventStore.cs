
using EzCommon.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage;

public interface IEventStore
{
    Task<EventStream> SaveAsync<T>(T aggregate) where T : AggregateRoot;
}

