using Confluent.Kafka;
using Newtonsoft.Json;
using RealEstate.Producer.Interfaces;

namespace RealEstate.Producer.Services
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly ProducerConfig _producerConfig;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(ProducerConfig producerConfig, ILogger<KafkaProducerService> logger)
        {
            _producerConfig = producerConfig;
            _logger = logger;
        }

        public async Task<bool> SendMessageAsync<T>(string topic, string key, T value)
        {
            var message = new Message<string, string>()
            {
                Key = key,
                Value = JsonConvert.SerializeObject(value)
            };

            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                try
                {
                    var deliveryResult = await producer.ProduceAsync(topic, message);
                    _logger.LogInformation($"Delivered message to {deliveryResult.TopicPartitionOffset}");
                    return true;
                }
                catch (ProduceException<string, string> e)
                {
                    _logger.LogError($"Delivery failed: {e.Error.Reason}");
                    return false;
                }
            }
        }
    }
}
