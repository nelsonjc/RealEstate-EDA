using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealEstate.Database.Entities;
using RealEstate.Shared.Constants;
using RealEstate.Shared.CustomEntities;
using RealEstate.Database;
using System.Text.Json;
using AutoMapper;

namespace RealEstate.Service
{
    public class Worker(IMapper mapper, ReportDbContext reportDbContext, ILogger<Worker> logger) : BackgroundService
    {

        private readonly ILogger<Worker> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ReportDbContext _reportDbContext = reportDbContext;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = "property-consumer",
                ClientId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            consumer.Subscribe(ConfigConstant.TOPIC_PROPERTY);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumedData = consumer.Consume(TimeSpan.FromSeconds(3));
                    if (consumedData is not null)
                    {
                        var property = JsonSerializer.Deserialize<Property>(consumedData.Message.Value);
                        _logger.LogInformation($"Consuming {property}");
                        PropertyReport er = _mapper.Map<PropertyReport>(property);
                        _reportDbContext.Properties.Add(er);
                        await _reportDbContext.SaveChangesAsync();
                    }
                    else
                        _logger.LogInformation("Nothing found to consume");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Nothing found to consume, Error: " + ex.Message);
                }
            }
        }
    }
}
