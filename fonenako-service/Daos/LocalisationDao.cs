using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using fonenako.DatabaseContexts;
using fonenako_service.Models;
using Microsoft.EntityFrameworkCore;

namespace fonenako_service.Daos
{
    public class LocalisationDao : ILocalisationDao
    {
        private readonly FonenakoDbContext _fonenakoDbContext;

        private static readonly Dictionary<string, Expression<Func<Localisation, object>>> SortableFieldsQueryMapper = new()
        {
            { nameof(Localisation.Name), localisation => localisation.Name },
            { nameof(Localisation.Type), localisation => localisation.Type },
        };

        public LocalisationDao(FonenakoDbContext fonenakoDbContext)
        {
            _fonenakoDbContext = fonenakoDbContext ?? throw new ArgumentNullException(nameof(fonenakoDbContext));
        }

        public async Task<int> CountLocalisationsFoundAsync(string nameFilter)
        {
            var queryable = _fonenakoDbContext.Localisations.AsQueryable();
            if(!string.IsNullOrWhiteSpace(nameFilter))
            {
                queryable = queryable.Where(localisation => localisation.Name.ToLower().Contains(nameFilter.ToLower()));
            }

            return await queryable.CountAsync();
        }

        public async Task<IEnumerable<Localisation>> SearchLocalisationsAsync(int pageSize, int pageIndex, string nameFilter, string orderBy, Order order)
        {
            if (pageSize < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageSize));
            }
            if (pageIndex < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageIndex));
            }
            if (!SortableFieldsQueryMapper.TryGetValue(orderBy, out var orderExpression))
            {
                throw new ArgumentException($"Order field '{orderBy}' is unknown or not sortable", nameof(orderBy));
            }

            var toSkiped = (pageIndex - 1) * pageSize;
            var localisationQueryable = _fonenakoDbContext.Localisations.AsNoTracking()
                                                                    .Include(localisation => localisation.Hierarchy)
                                                                    .AsQueryable();
            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                localisationQueryable = localisationQueryable.Where(localisation => localisation.Name.ToLower().Contains(nameFilter.ToLower()));
            }
            localisationQueryable = order == Order.Asc ? localisationQueryable.OrderBy(orderExpression) : localisationQueryable.OrderByDescending(orderExpression);
            return await localisationQueryable.Skip(toSkiped).Take(pageSize).ToArrayAsync();
        }
    }
}
