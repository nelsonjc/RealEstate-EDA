using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealEstate.Application.Filters;
using RealEstate.Application.Interfaces;
using RealEstate.Database.Entities;
using RealEstate.Shared;
using RealEstate.Shared.Constants;
using RealEstate.Shared.Exceptions;
using System.Net;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController(
        IPropertyBusinessLogic propertyBL,
        ILogger<ReportController> logger) : ControllerBase
    {
        private readonly ILogger<ReportController> _logger = logger;
        private readonly IPropertyBusinessLogic _propertyBL = propertyBL;

        /// <summary>
        /// Retrieves property details by ID.
        /// </summary>
        /// <param name="id">The ID of the property to retrieve.</param>
        /// <returns>Property details or an error if something went wrong.</returns>
        [HttpGet("get-byid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var property = await _propertyBL.GetById(id);

                if (property != null)
                {
                    return Ok(property);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, MessageConstant.PROPERTY_GETBYID_ERROR_MESSAGE, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of properties based on filters.
        /// </summary>
        /// <param name="filters">Query filters for properties, such as page size, search term, etc.</param>
        /// <returns>Paginated list of properties or an error if something went wrong.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> All([FromQuery] PropertyQueryFilter filters)
        {
            try
            {
                var dataResult = await _propertyBL.GetPaginated(filters);

                if (dataResult != null && dataResult.Count() > 0)
                {
                    var infoResponse = dataResult.Select(x => new PropertyReportAllDto()
                    {
                        IdOwner = x.IdOwner,
                        Name = x.Name,
                        Id = x.Id,
                        IdProperty = x.IdProperty,
                        Active = x.Active,
                        Address = x.Address,
                        CodeInternal = x.CodeInternal,
                        Images = x.Images != null ? x.Images.Count() : 0,
                        Owner = x.Owner.Name,
                        Price = x.Price,
                        Traces = x.Traces !=  null ?  x.Traces.Count(): 0,
                        Year = x.Year,
                    });

                    var metadata = new
                    {
                        dataResult.TotalCount,
                        dataResult.PageSize,
                        dataResult.CurrentPage,
                        dataResult.TotalPages,
                        dataResult.HasPreviousPage,
                        dataResult.HasNextPage,
                    };

                    Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

                    return Ok(new
                    {
                        data = infoResponse,
                        metadata
                    });
                }

                return NoContent();

            }
            catch (Exception ex)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, MessageConstant.PROPERTY_PAGINATED_ERROR_MESSAGE, ex.Message);
            }
        }
    }
}
