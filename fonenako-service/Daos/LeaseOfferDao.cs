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
            { nameof(LeaseOffer.MonthlyRent), offer => offer.MonthlyRent }
        };

        private static readonly Dictionary<string, Func<IQueryable<LeaseOffer>, object, IQueryable<LeaseOffer>>> FilterableFieldsQueryMap = new()
        {
            { LeaseOfferFilterFields.SurfaceMin, ApplySurfaceMinFilterQuery },
            { LeaseOfferFilterFields.SurfaceMax, ApplySurfaceMaxFilterQuery },
            { LeaseOfferFilterFields.MonthlyRentMin, ApplyMonthlyRentMinFilterQuery },
            { LeaseOfferFilterFields.MonthlyRentMax, ApplyMonthlyRentMaxFilterQuery },
            { LeaseOfferFilterFields.RoomsMin, ApplyRoomsMinFilterQuery },
            { LeaseOfferFilterFields.RoomsMax, ApplyRoomsMaxFilterQuery }
        };

        public LeaseOfferDao(FonenakoDbContext fonenakoDbContext)
        {
            _fonenakoDbContext = fonenakoDbContext ?? throw new ArgumentNullException(nameof(fonenakoDbContext));
        }

        public async Task<int> CountLeaseOffersAsync(IDictionary<string, object> filterMap)
        {
            if(filterMap == null)
            {
                throw new ArgumentNullException(nameof(filterMap));
            }

            return await ComputeFilterQueryable(filterMap).CountAsync();
        }

        public async Task InitAsync()
        {
            await _fonenakoDbContext.LeaseOffers.AddRangeAsync(FakeData.FakeDatas());
            _fonenakoDbContext.SaveChanges();
        }

        public async Task<IEnumerable<LeaseOffer>> RetrieveLeaseOffersByPageAsync(int pageSize, int pageIndex, IDictionary<string, object> filterMap, string orderBy, Order order)
        {
            if (filterMap == null)
            {
                throw new ArgumentNullException(nameof(filterMap));
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
            var leaseOfferQueryable = ComputeFilterQueryable(filterMap);
            leaseOfferQueryable = order == Order.Asc ? leaseOfferQueryable.OrderBy(orderExpression) : leaseOfferQueryable.OrderByDescending(orderExpression);
            return await leaseOfferQueryable.Skip(toSkiped).Take(pageSize).ToArrayAsync();
        }

        public async Task InsertManyAsync(IEnumerable<LeaseOffer> leaseOffers)
        {
            await _fonenakoDbContext.LeaseOffers.AddRangeAsync(leaseOffers);
            _fonenakoDbContext.SaveChanges();
        }

        public async Task InsertAsync(LeaseOffer leaseOffer)
        {
            await _fonenakoDbContext.LeaseOffers.AddAsync(leaseOffer);
            _fonenakoDbContext.SaveChanges();
        }

        public async Task<LeaseOffer> FindLeaseOfferByIdAsync(int leaseOfferId)
        {
            if(leaseOfferId < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(leaseOfferId));
            }

            return await _fonenakoDbContext.LeaseOffers.Where(leaseOffer => leaseOffer.LeaseOfferID == leaseOfferId).FirstOrDefaultAsync();
        }

        private IQueryable<LeaseOffer> ComputeFilterQueryable(IDictionary<string, object> filterMap)
        {
            var currentQuery = _fonenakoDbContext.LeaseOffers.AsQueryable();
            foreach (var fieldMap in filterMap)
            {
                if (!FilterableFieldsQueryMap.TryGetValue(fieldMap.Key, out var query))
                {
                    throw new ArgumentException($"Field with name : '{fieldMap.Key}' not found as filterable");
                }

                currentQuery = query(currentQuery, fieldMap.Value);
            }

            return currentQuery;
        }

        private static IQueryable<LeaseOffer> ApplySurfaceMinFilterQuery(IQueryable<LeaseOffer> queryable, object filterValue)
        {
            if(!(filterValue is double))
            {
                throw new InvalidOperationException($"SurfaceMin should be a type of {typeof(double).Name}");
            }

            var castedValue = (double)filterValue;
            return queryable.Where(offer => offer.Surface >= castedValue);
        }

        private static IQueryable<LeaseOffer> ApplySurfaceMaxFilterQuery(IQueryable<LeaseOffer> queryable, object filterValue)
        {
            if (!(filterValue is double))
            {
                throw new InvalidOperationException($"SurfaceMax should be a type of {typeof(double).Name}");
            }

            var castedValue = (double)filterValue;
            return queryable.Where(offer => offer.Surface <= castedValue);
        }

        private static IQueryable<LeaseOffer> ApplyMonthlyRentMinFilterQuery(IQueryable<LeaseOffer> queryable, object filterValue)
        {
            if (!(filterValue is double))
            {
                throw new InvalidOperationException($"MonthlyRentMin should be a type of {typeof(double).Name}");
            }

            var castedValue = (double)filterValue;
            return queryable.Where(offer => offer.MonthlyRent >= castedValue);
        }

        private static IQueryable<LeaseOffer> ApplyMonthlyRentMaxFilterQuery(IQueryable<LeaseOffer> queryable, object filterValue)
        {
            if (!(filterValue is double))
            {
                throw new InvalidOperationException($"MonthlyRentMax should be a type of {typeof(double).Name}");
            }

            var castedValue = (double)filterValue;
            return queryable.Where(offer => offer.MonthlyRent <= castedValue);
        }

        private static IQueryable<LeaseOffer> ApplyRoomsMinFilterQuery(IQueryable<LeaseOffer> queryable, object filterValue)
        {
            if (!(filterValue is int))
            {
                throw new InvalidOperationException($"RoomsMin should be a type of {typeof(int).Name}");
            }

            var castedValue = (int)filterValue;
            return queryable.Where(offer => offer.Rooms >= castedValue);
        }

        private static IQueryable<LeaseOffer> ApplyRoomsMaxFilterQuery(IQueryable<LeaseOffer> queryable, object filterValue)
        {
            if (!(filterValue is int))
            {
                throw new InvalidOperationException($"RoomsMax should be a type of {typeof(int).Name}");
            }

            var castedValue = (int)filterValue;
            return queryable.Where(offer => offer.Rooms <= castedValue);
        }
    }
}
