using Confluent.Kafka;

namespace RealEstate.Producer.App_Start
{
    internal static class KafkaConfiguration
    {
        internal static void AddKafkaConfiguration(this IServiceCollection services)
        {
            // Configurar Kafka ProducerConfig
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };

            // Registrar el config y el servicio de Kafka
            services.AddSingleton(producerConfig);
        }
    }
}
