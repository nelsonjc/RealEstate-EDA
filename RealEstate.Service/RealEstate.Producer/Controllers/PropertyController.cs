using Microsoft.AspNetCore.Mvc;
using RealEstate.Producer.Interfaces;
using RealEstate.Producer.Requests;
using RealEstate.Shared.Constants;
using RealEstate.Shared.CustomEntities;

namespace RealEstate.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyController(
        IKafkaProducerService kafkaProducerService,
        ILogger<PropertyController> logger) : ControllerBase
    {
        private readonly IKafkaProducerService _kafkaProducerService = kafkaProducerService;
        private readonly ILogger<PropertyController> _logger = logger;


        [HttpPost]
        public async Task<ActionResult<Property>> CreateProperty(PropertyCreationRequest request)
        {
            Guid idProperty = Guid.NewGuid();
            var owner = new Owner(request.Owner.Document, request.Owner.Name, request.Owner.Address, request.Owner.Photo, request.Owner.Birthday);       

            var property = new Property(idProperty, request.Name, request.Address, request.Price, request.CodeInternal, request.Year, owner.IdOwner, request.Active);
            property.Owner = owner;

            // Traces
            request.Traces.ToList().ForEach(x => property.Traces.Add(new PropertyTrace(idProperty, x.DateSale, x.Name, x.Value, x.Tax)));

            //Images
            request.Images.ToList().ForEach(x => property.Images.Add(new PropertyImage(idProperty, x.FileBase64, x.Enable)));

            try
            {
                // Enviar el mensaje de propiedad a Kafka usando el servicio de Kafka
                bool messageSent = await _kafkaProducerService.SendMessageAsync(ConfigConstant.TOPIC_PROPERTY, idProperty.ToString(), property);

                if (!messageSent)
                {
                    _logger.LogError("Failed to send property to Kafka");
                    return StatusCode(500, "Error sending message to Kafka");
                }

                return CreatedAtAction(nameof(CreateProperty), new { id = idProperty }, property);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the property.");
                return BadRequest("An error occurred while creating the property.");
            }
        }
    }
}
