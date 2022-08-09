using Confluent.Kafka;
using EzCommon.Events;
using MediatR;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EzCommon.Consumers
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IEventRegister _eventRegister;

        public KafkaConsumerBackgroundService(IMediator mediator, IEventRegister eventRegister)
        {
            _mediator = mediator;
            _eventRegister = eventRegister;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = $"ezgym-service",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };


            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe("ezgym-events");


            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Factory.StartNew(() =>
                {
                    var result = consumer.Consume(stoppingToken);
                    var eventType = _eventRegister.GetRegisterType(result.Message.Key);

                    var @event = JsonSerializer.Deserialize(result.Message.Value, eventType);

                    _mediator.Publish(@event);
                });
            }
        }
    }
}
