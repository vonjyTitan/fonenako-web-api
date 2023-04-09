
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fonenako_service.Daos;
using fonenako_service.Dtos;
using fonenako_service.Models;

namespace fonenako_service.Services
{
    public class CityService : ICityService
    {
        private readonly ICityDao _cityDao;

        private readonly IMapper _mapper;

        private static readonly Dictionary<string, string> OrdereableFieldsMap = new()
        {
            { nameof(CityDto.Name), nameof(City.Name) },
        };

        public CityService(ICityDao cityDao, IMapper mapper)
        {
            _cityDao = cityDao ?? throw new ArgumentNullException(nameof(cityDao));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Pageable<CityDto>> RetrieveCitiesAsync(int pageSize, int pageIndex, CityFilter filter, string orderBy, Order order)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageIndex));
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageSize));
            }

            if (!OrdereableFieldsMap.TryGetValue(orderBy, out var orderModelFieldName))
            {
                throw new ArgumentException($"The dto field : {orderBy} is unknown or not sortable", nameof(orderBy));
            }

            var total = await _cityDao.CountCitiesAsync(filter);
            var totalPage = (int)Math.Round(total / (double)pageSize, MidpointRounding.ToPositiveInfinity);
            if (totalPage == 0)
            {
                return new Pageable<CityDto>(1, pageSize, totalPage, 0, Array.Empty<CityDto>());
            }

            pageIndex = totalPage < pageIndex ? totalPage : pageIndex;

            var retrievedOffers = await _cityDao.RetrieveCitiesByPageAsync(pageSize, pageIndex, filter, orderModelFieldName, order);
            return new Pageable<CityDto>(pageIndex, pageSize, totalPage, total, _mapper.Map<IEnumerable<CityDto>>(retrievedOffers));
        }
    }
}
