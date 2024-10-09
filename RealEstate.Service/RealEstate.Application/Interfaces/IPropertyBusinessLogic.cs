using RealEstate.Application.Filters;
using RealEstate.Database.Entities;
using RealEstate.Shared.CustomEntities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyBusinessLogic
    {
        /// <summary>
        /// Adds a new property entity to the database.
        /// </summary>
        /// <param name="entity">The property entity to add.</param>
        /// <returns>The ID of the newly added property.</returns>
        Task<Guid> AddProperty(PropertyReport entity);

        /// <summary>
        /// Adds a new property image entity to the database.
        /// </summary>
        /// <param name="entity">The property image entity to add.</param>
        /// <returns>The ID of the newly added property image.</returns>
        Task<Guid> AddImageProperty(PropertyImageReport entity);

        /// <summary>
        /// Retrieves a property entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property.</param>
        /// <returns>The property entity if found, or null if not found.</returns>
        Task<PropertyReport?> GetById(Guid id);

        /// <summary>
        /// Retrieves a paginated list of properties based on the provided filters.
        /// </summary>
        /// <param name="filters">The filters to apply for retrieving the paginated list of properties.</param>
        /// <returns>A paginated list of properties that match the specified filters.</returns>
        Task<PagedList<PropertyReport>> GetPaginated(PropertyQueryFilter filters);

        /// <summary>
        /// Update  a property entity to the database.
        /// </summary>
        /// <param name="entity">The property entity to add.</param>
        /// <returns>The ID of the newly added property.</returns>
        Task<Guid> UpdateProperty(PropertyReport entity);
    }
}
