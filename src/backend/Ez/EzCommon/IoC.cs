using EzCommon.Events;
using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using Google.Cloud.Storage.V1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EzCommon
{
    public static class IoC
    {
        public static IServiceCollection AddEzCommon(this IServiceCollection services, params Type[] mediatrTypes)
        {
            services.AddMediatR(mediatrTypes);
            services.AddSingleton<IBus, KafkaBus>();
            services.AddSingleton<IFileStorage>(new GCPFileStorage(StorageClient.Create(), "ezgym"));

            return services;
        }

    }
}
