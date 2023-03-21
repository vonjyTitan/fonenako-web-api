using System.Collections.Generic;
using System.Threading.Tasks;
using fonenako_service.Daos;
using fonenako_service.Dtos;

namespace fonenako_service.Services
{
    public interface ILeaseOfferService
    {
        Task<Pageable<LeaseOfferDto>> RetrieveLeaseOffersAsync(int pageSize, int pageIndex, IDictionary<string, object> filterMap, string orderBy, Order order);

        Task<LeaseOfferDto> FindLeaseOfferByIdAsync(int leaseOfferId);

        Task InitAsync();
    }
}
