using Confluent.Kafka;
using EzCommon.Events;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EzCommon.Infra.Bus
{
    public class KafkaBus : IBus
    {
        public async Task PublishAsync<TEvent>(params TEvent[] events) where TEvent : IEvent
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using var producer = new ProducerBuilder<string, string>(config).Build();

            var publishTasks = events.ToList().Select(@event =>
            {
                var eventJson = JsonSerializer.Serialize(@event, @event.GetType());
                return producer.ProduceAsync("ezgym-events", new Message<string, string> { Key = @event.GetType().Name, Value = eventJson });
            });


            await Task.WhenAll(publishTasks);
        }
    }
}
