
using System.Threading.Tasks;
using fonenako_service.Daos;
using fonenako_service.Dtos;

namespace fonenako_service.Services
{
    public interface ILocalisationService
    {
        Task<Pageable<LocalisationDto>> SearchLocalisationsAsync(int pageSize, int pageIndex, string nameFilter, string orderBy, Order order);
    }
}
