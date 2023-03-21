using System.Collections.Generic;
using System.Threading.Tasks;
using fonenako.Models;

namespace fonenako_service.Daos
{
    public interface ILeaseOfferDao
    {
        Task<IEnumerable<LeaseOffer>> RetrieveLeaseOffersByPageAsync(int pageSize, int pageIndex, IDictionary<string, object> filterMap, string orderBy, Order order);

        Task<int> CountLeaseOffersAsync(IDictionary<string, object> filterMap);

        Task<LeaseOffer> FindLeaseOfferById(int leaseOfferId);

        Task InsertManyAsync(IEnumerable<LeaseOffer> leaseOffers);

        Task InsertAsync(LeaseOffer leaseOffer);
    }
}
