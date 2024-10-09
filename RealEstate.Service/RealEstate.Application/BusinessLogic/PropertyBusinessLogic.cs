using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealEstate.Application.Filters;
using RealEstate.Application.Interfaces;
using RealEstate.Database;
using RealEstate.Database.Entities;
using RealEstate.Shared.CustomEntities;
using RealEstate.Shared.Interfaces;
using RealEstate.Shared.Options;
using System.Linq.Expressions;

namespace RealEstate.Application.BusinessLogic
{
    public class PropertyBusinessLogic : IPropertyBusinessLogic
    {
        private readonly ReportDbContext _dbContext;
        private readonly PaginationOptions _paginationOptions;
        private readonly ITreeModifier _treeModifier;

        public PropertyBusinessLogic(ReportDbContext reportDbContext, IOptions<PaginationOptions> paginationOptions, ITreeModifier treeModifier)
        {
            _dbContext = reportDbContext;
            _paginationOptions = paginationOptions.Value;
            _treeModifier = treeModifier;
        }

        #region Public Methods
        /// <inheritdoc/>

        public async Task<Guid> AddImageProperty(PropertyImageReport entity)
        {
            await _dbContext.PropertyImages.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<Guid> AddProperty(PropertyReport entity)
        {
            await _dbContext.Properties.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<PropertyReport?> GetById(Guid id)
        {
            return await _dbContext.Properties.Select(x => new PropertyReport()
            {
                IdOwner = x.IdOwner,
                Name = x.Name,
                Id = x.Id,
                IdProperty = x.IdProperty,
                Active = x.Active,
                Address = x.Address,
                CodeInternal = x.CodeInternal,
                Images = x.Images,
                Owner = x.Owner,
                Price = x.Price,
                Traces = x.Traces,
                Year = x.Year,
            }).Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        }

        /// <inheritdoc/>
        public async Task<PagedList<PropertyReport>> GetPaginated(PropertyQueryFilter filters)
        {
            PagedList<PropertyReport> infoResult;

            //Validation pagination
            if (filters.PageNumber == null || filters.PageNumber == 0)
                filters.PageNumber = _paginationOptions.DefaultPageNumber;

            if (filters.RowsOfPage == null || filters.RowsOfPage == 0)
                filters.RowsOfPage = _paginationOptions.DefaultPageSize;

            if (filters.AllRows)
                filters.RowsOfPage = int.MaxValue;

            if (!filters.OrderAsc.HasValue)
                filters.OrderAsc = false;

            var where = GenerateWhereFilter(filters);

            var includes = new Expression<Func<PropertyReport, object>>[]
            {
                x => x.Owner,
                x => x.Images,
                x => x.Traces
            };

            infoResult = await GetPaginated(where, x => x.Id, filters.OrderAsc.Value, (int)filters.PageNumber, (int)filters.RowsOfPage, includes);

            return infoResult;
        }

        /// <inheritdoc/>
        public async Task<Guid> UpdateProperty(PropertyReport entity)
        {
            _dbContext.Properties.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Generates a dynamic 'where' filter for <see cref="Property"/> entities based on the specified <see cref="PropertyQueryFilter"/>.
        /// The filter is constructed using the provided criteria, combining conditions with logical 'AND'.
        /// </summary>
        /// <param name="filters">The <see cref="PropertyQueryFilter"/> containing the filtering criteria.</param>
        /// <returns>An <see cref="Expression{Func{T, TResult}}"/> representing the combined 'where' filter for the <see cref="Property"/> query.</returns>
        private Expression<Func<PropertyReport, bool>> GenerateWhereFilter(PropertyQueryFilter filters)
        {
            Expression<Func<PropertyReport, bool>> where = x => x.Active;

            if (!string.IsNullOrEmpty(filters.Name))
                where = _treeModifier.CombineAnd(where, x => x.Name.ToLower().Contains(filters.Name.ToLower()));

            if (!string.IsNullOrEmpty(filters.Address))
                where = _treeModifier.CombineAnd(where, x => x.Address.ToLower().Contains(filters.Address.ToLower()));

            if (filters.PriceInitial.HasValue && filters.PriceInitial.Value > 0 && filters.PriceFinish.HasValue)
                where = _treeModifier.CombineAnd(where, x => x.Price >= filters.PriceInitial.Value && x.Price <= filters.PriceFinish.Value);

            if (!string.IsNullOrEmpty(filters.CodeInternal))
                where = _treeModifier.CombineAnd(where, x => x.CodeInternal.ToLower().Contains(filters.CodeInternal.ToLower()));

            if (filters.Year.HasValue)
                where = _treeModifier.CombineAnd(where, x => x.Year == filters.Year);

            if (!string.IsNullOrEmpty(filters.OwnerName))
                where = _treeModifier.CombineAnd(where, x => x.Owner.Name.ToLower().Contains(filters.OwnerName.ToLower()));

            return where;
        }

        private async Task<PagedList<PropertyReport>> GetPaginated(
            Expression<Func<PropertyReport, bool>> where,
            Expression<Func<PropertyReport, object>> orderBy,
            bool orderByAsc, int pageNumber, int pageSize,
            params Expression<Func<PropertyReport, object>>[] propertyNames)
        {
            IQueryable<PropertyReport> query;
            int totalItems;

            query = where is null
                ? _dbContext.Set<PropertyReport>().AsQueryable()
                : _dbContext.Set<PropertyReport>().Where(where).AsQueryable();

            totalItems = await query.CountAsync();

            query = orderByAsc
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            if (propertyNames != null)
            {
                query = ValidateInclude(propertyNames, query);
            }

            try
            {
                var items = await query.ToListAsync();
                return new PagedList<PropertyReport>(items, totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al ejecutar la consulta: {ex.Message}", ex);
            }
        }

        private IQueryable<PropertyReport> ValidateInclude(
            Expression<Func<PropertyReport, object>>[] propertyNames,
            IQueryable<PropertyReport> query)
        {
            foreach (var item in propertyNames)
            {
                if (item.Body is MemberExpression memberExpression)
                {
                    query = query.Include(item);
                }
                else if (item.Body is MethodCallExpression methodCallExpression)
                {
                    query = IncludeNestedProperty(query, methodCallExpression);
                }
                else
                {
                    throw new InvalidOperationException("The expression 'item' should represent a property access.");
                }
            }

            return query;
        }

        private IQueryable<PropertyReport> IncludeNestedProperty(
            IQueryable<PropertyReport> query,
            MethodCallExpression expression)
        {
            var includePath = string.Join(".", GetIncludePath(expression));
            return query.Include(includePath);
        }

        private IEnumerable<string> GetIncludePath(Expression expression)
        {
            while (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Arguments.FirstOrDefault() is MemberExpression memberExpression)
                {
                    yield return memberExpression.Member.Name;
                    expression = methodCallExpression.Arguments.LastOrDefault();
                }
            }

            if (expression is MemberExpression memberExpressionFinal)
            {
                yield return memberExpressionFinal.Member.Name;
            }
        }

        #endregion
    }
}
