using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EzCommon
{
    public static class IoC
    {
        public static IServiceCollection AddEzCommon(this IServiceCollection services, params Type[] mediatrTypes)
        {
            services.AddSingleton<IEventStore, StoreInLocal>();
            services.AddSingleton<IQueryStorage, StoreInLocal>();
            services.AddMediatR(mediatrTypes);
            services.AddSingleton<IBus, InMemoryBus>();

            return services;
        }

    }
}
