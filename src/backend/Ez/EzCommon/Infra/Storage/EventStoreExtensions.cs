using System;
using System.Threading.Tasks;
using Marten.Events.Daemon;

namespace EzCommon.Infra.Storage
{
    public static class EventStoreExtensions
    {
        public static async Task UseDaemonProjectionAsync(this IEventStore store, Action<IProjectionDaemon> useDaemon)
        {
            using var daemon = await store.BuildProjectionDaemonAsync();
            useDaemon(daemon);
        }
    }
}
