using RealEstate.Producer.Interfaces;
using RealEstate.Producer.Services;

namespace RealEstate.Producer.App_Start
{
    internal static class DIConfiguration
    {
        internal static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddTransient<IKafkaProducerService, KafkaProducerService>();
        }
    }
}
