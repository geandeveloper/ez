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

            services.AddScoped<IBus, InMemoryBus>();
            services.AddScoped<IFileStorage>((_) => new GCPFileStorage(StorageClient.Create(), "ezgym"));

          
            return services;
        }

    }


}
