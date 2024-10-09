namespace RealEstate.Producer.Interfaces
{
    public interface IKafkaProducerService
    {
        Task<bool> SendMessageAsync<T>(string topic, string key, T value);
    }
}
