using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fonenako_service.Daos;
using fonenako_service.Dtos;
using fonenako_service.Models;

namespace fonenako_service.Services
{
    public class LocalisationService : ILocalisationService
    {
        private readonly ILocalisationDao _localisationDao;

        private readonly IMapper _mapper;

        private static readonly Dictionary<string, string> SortableFieldsMap = new()
        {
            { nameof(LocalisationDto.Name), nameof(Localisation.Name) },
            { nameof(LocalisationDto.Type), nameof(Localisation.Type) }
        };

        public LocalisationService(ILocalisationDao localisationDao, IMapper mapper)
        {
            _localisationDao = localisationDao ?? throw new ArgumentNullException(nameof(localisationDao));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Pageable<LocalisationDto>> SearchLocalisationsAsync(int pageSize, int pageIndex, string nameFilter, string orderBy, Order order)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageIndex));
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Value cannot be less than 1", nameof(pageSize));
            }

            if (!SortableFieldsMap.TryGetValue(orderBy, out var orderModelFieldName))
            {
                throw new ArgumentException($"The dto field : {orderBy} is unknown or not sortable", nameof(orderBy));
            }

            var total = await _localisationDao.CountLocalisationsFoundAsync(nameFilter);
            var totalPage = (int)Math.Round(total / (double)pageSize, MidpointRounding.ToPositiveInfinity);
            if (totalPage == 0)
            {
                return new Pageable<LocalisationDto>(1, pageSize, totalPage, 0, Array.Empty<LocalisationDto>());
            }

            pageIndex = totalPage < pageIndex ? totalPage : pageIndex;

            var retrievedOffers = await _localisationDao.SearchLocalisationsAsync(pageSize, pageIndex, nameFilter, orderModelFieldName, order);
            return new Pageable<LocalisationDto>(pageIndex, pageSize, totalPage, total, _mapper.Map<IEnumerable<LocalisationDto>>(retrievedOffers));
        }
    }
}
