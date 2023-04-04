using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fonenako.Models;
using fonenako_service.Daos;
using fonenako_service.Dtos;

namespace fonenako_service.Services
{
    public class LeaseOfferService : ILeaseOfferService
    {
        private readonly ILeaseOfferDao _leaseOfferDao;

        private readonly IMapper _mapper;

        private static readonly Dictionary<string, string> OrdereableFieldsMap = new()
        {
            { nameof(LeaseOfferDto.LeaseOfferID), nameof(LeaseOffer.LeaseOfferID)},
            { nameof(LeaseOfferDto.Surface), nameof(LeaseOffer.Surface)},
            { nameof(LeaseOfferDto.MonthlyRent), nameof(LeaseOffer.MonthlyRent) },
            { nameof(LeaseOfferDto.CreationDate), nameof(LeaseOffer.CreationDate) }
        };

        public LeaseOfferService(ILeaseOfferDao leaseOfferDao, IMapper mapper)
        {
            _leaseOfferDao = leaseOfferDao ?? throw new ArgumentNullException(nameof(leaseOfferDao));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Pageable<LeaseOfferDto>> RetrieveLeaseOffersAsync(int pageSize, int pageIndex, LeaseOfferFilter filter, string orderBy, Order order)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageIndex));
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageSize));
            }

            if(!OrdereableFieldsMap.TryGetValue(orderBy, out var orderModelFieldName))
            {
                throw new ArgumentException($"The dto field : {orderBy} is unknown or not sortable", nameof(orderBy));
            }

            var total = await _leaseOfferDao.CountLeaseOffersAsync(filter);
            var totalPage = (int) Math.Round(total / (double)pageSize, MidpointRounding.ToPositiveInfinity);
            if(totalPage == 0)
            {
                return new Pageable<LeaseOfferDto>(1, pageSize, totalPage, 0, Array.Empty<LeaseOfferDto>());
            }

            pageIndex = totalPage < pageIndex ? totalPage : pageIndex;

            var retrievedOffers = await _leaseOfferDao.RetrieveLeaseOffersByPageAsync(pageSize, pageIndex, filter, orderModelFieldName, order);
            return new Pageable<LeaseOfferDto>(pageIndex, pageSize, totalPage, total, _mapper.Map<IEnumerable<LeaseOfferDto>>(retrievedOffers));
        }

        public async Task InitAsync()
        {
            await _leaseOfferDao.InsertManyAsync(FakeData.FakeDatas());
        }

        public async Task<LeaseOfferDto> FindLeaseOfferByIdAsync(int leaseOfferId)
        {
            var leaseOffer = await _leaseOfferDao.FindLeaseOfferByIdAsync(leaseOfferId);
            return leaseOffer != null ? _mapper.Map<LeaseOfferDto>(leaseOffer) : null;
        }
    }
}
