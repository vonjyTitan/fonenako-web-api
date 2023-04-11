using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using fonenako.DatabaseContexts;
using fonenako.Models;
using Microsoft.EntityFrameworkCore;

namespace fonenako_service.Daos
{
    public class LeaseOfferDao : ILeaseOfferDao
    {
        private readonly FonenakoDbContext _fonenakoDbContext;

        private static readonly Dictionary<string, Expression<Func<LeaseOffer, object>>> OrderableFieldsQueryMapper = new()
        {
            { nameof(LeaseOffer.LeaseOfferID), offer => offer.LeaseOfferID},
            { nameof(LeaseOffer.Surface), offer => offer.Surface },
            { nameof(LeaseOffer.MonthlyRent), offer => offer.MonthlyRent },
            { nameof(LeaseOffer.CreationDate), offer => offer.CreationDate }
        };

        public LeaseOfferDao(FonenakoDbContext fonenakoDbContext)
        {
            _fonenakoDbContext = fonenakoDbContext ?? throw new ArgumentNullException(nameof(fonenakoDbContext));
        }

        public async Task<int> CountLeaseOffersAsync(LeaseOfferFilter filter)
        {
            if(filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return await ComputeFilterQueryable(_fonenakoDbContext.LeaseOffers, filter).CountAsync();
        }

        public async Task<IEnumerable<LeaseOffer>> RetrieveLeaseOffersByPageAsync(int pageSize, int pageIndex, LeaseOfferFilter filter, string orderBy, Order order)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageSize));
            }
            if (pageIndex < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageIndex));
            }
            if(!OrderableFieldsQueryMapper.TryGetValue(orderBy, out var orderExpression))
            {
                throw new ArgumentException($"Order field '{orderBy}' is unknown or not orderable", nameof(orderBy));
            }

            var toSkiped = (pageIndex - 1) * pageSize;
            var leaseOfferQueryable = ComputeFilterQueryable(_fonenakoDbContext.LeaseOffers.AsNoTracking()
                                                                                           .Include(leaseOffer => leaseOffer.Localisation)
                                                                                           .ThenInclude(localisation => localisation.Hierarchy), filter);
            leaseOfferQueryable = order == Order.Asc ? leaseOfferQueryable.OrderBy(orderExpression) : leaseOfferQueryable.OrderByDescending(orderExpression);
            return await leaseOfferQueryable.Skip(toSkiped).Take(pageSize).ToArrayAsync();
        }

        public async Task<LeaseOffer> FindLeaseOfferDetailsByIdAsync(int leaseOfferId)
        {
            if(leaseOfferId < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(leaseOfferId));
            }

            return await _fonenakoDbContext.LeaseOffers.AsNoTracking()
                                                       .Include(leaseOffer => leaseOffer.Localisation)
                                                       .ThenInclude(localisation => localisation.Hierarchy)
                                                       .Where(leaseOffer => leaseOffer.LeaseOfferID == leaseOfferId)
                                                       .FirstOrDefaultAsync();
        }

        private static IQueryable<LeaseOffer> ComputeFilterQueryable(IQueryable<LeaseOffer> currentQuery, LeaseOfferFilter filter)
        {
            if (filter == LeaseOfferFilter.Default) return currentQuery;

            return currentQuery.Where(leaseOffer => (filter.Rooms.Length == 0 || filter.Rooms.Contains(leaseOffer.Rooms)) &&
            (!filter.MonthlyRentMin.HasValue || leaseOffer.MonthlyRent >= filter.MonthlyRentMin) &&
            (!filter.MonthlyRentMax.HasValue || leaseOffer.MonthlyRent <= filter.MonthlyRentMax) &&
            (!filter.SurfaceMin.HasValue || leaseOffer.Surface >= filter.SurfaceMin) &&
            (!filter.SurfaceMax.HasValue || leaseOffer.Surface <= filter.SurfaceMax) &&
            (filter.Localisations.Length == 0 || filter.Localisations.Contains(leaseOffer.Localisation.LocalisationId)));
        }
    }
}
