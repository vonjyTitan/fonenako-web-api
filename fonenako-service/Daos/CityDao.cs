
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
    public class CityDao : ICityDao
    {
        private readonly FonenakoDbContext _fonenakoDbContext;

        private static readonly Dictionary<string, Expression<Func<City, object>>> OrderableFieldsQueryMapper = new()
        {
            { nameof(City.Name), city => city.Name }
        };

        public CityDao(FonenakoDbContext fonenakoDbContext)
        {
            _fonenakoDbContext = fonenakoDbContext ?? throw new ArgumentNullException(nameof(fonenakoDbContext));
        }

        public async Task<int> CountCitiesAsync(CityFilter filter)
        {
            if(filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }


            return await _fonenakoDbContext.Cities.Where(city => !string.IsNullOrWhiteSpace(filter.Name) && city.Name.ToLower().Contains(filter.Name.ToLower())).CountAsync();
        }

        public async Task<IEnumerable<City>> RetrieveCitiesByPageAsync(int pageSize, int pageIndex, CityFilter filter, string orderBy, Order order)
        {
            throw new System.NotImplementedException();
        }
    }
}
