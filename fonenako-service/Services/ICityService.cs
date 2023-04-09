
using System.Threading.Tasks;
using fonenako_service.Daos;
using fonenako_service.Dtos;

namespace fonenako_service.Services
{
    public interface ICityService
    {
        Task<Pageable<CityDto>> RetrieveCitiesAsync(int pageSize, int pageIndex, CityFilter filter, string orderBy, Order order);
    }
}
