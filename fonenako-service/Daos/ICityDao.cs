
using System.Collections.Generic;
using System.Threading.Tasks;
using fonenako_service.Models;

namespace fonenako_service.Daos
{
    public interface ICityDao
    {
        Task<IEnumerable<City>> RetrieveCitiesByPageAsync(int pageSize, int pageIndex, CityFilter filter, string orderBy, Order order);

        Task<int> CountCitiesAsync(CityFilter filter);
    }
}
