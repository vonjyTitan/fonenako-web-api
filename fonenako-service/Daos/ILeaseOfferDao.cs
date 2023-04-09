using System.Collections.Generic;
using System.Threading.Tasks;
using fonenako.Models;

namespace fonenako_service.Daos
{
    public interface ILeaseOfferDao
    {
        Task<IEnumerable<LeaseOffer>> RetrieveLeaseOffersByPageAsync(int pageSize, int pageIndex, LeaseOfferFilter filter, string orderBy, Order order);

        Task<int> CountLeaseOffersAsync(LeaseOfferFilter filter);

        Task<LeaseOffer> FindLeaseOfferDetailsByIdAsync(int leaseOfferId);
    }
}
