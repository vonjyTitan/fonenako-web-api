
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using fonenako_service.Daos;
using fonenako_service.Dtos;
using fonenako_service.Properties;
using fonenako_service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace fonenako_service.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/V1/Localisations")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public class LocalisationController : ControllerBase
    {
        private readonly ILocalisationService _localisationService;

        private readonly FunctionalSettings _functionalSettings;

        private static readonly Dictionary<string, string> OrdereableFieldsMap = new()
        {
            { LocalisationDtoProperties.Name, nameof(LocalisationDto.Name) },
            { LocalisationDtoProperties.Type, nameof(LocalisationDto.Type) },
        };

        public LocalisationController(ILocalisationService localisationService, IOptions<FunctionalSettings> options)
        {
            _localisationService = localisationService ?? throw new ArgumentNullException(nameof(localisationService));
            _functionalSettings = options?.Value ?? throw new ArgumentException("Argument or its 'Value' is null", nameof(options));
        }

        [HttpGet(Name = "Search localisations")]
        [ProducesResponseType(typeof(Pageable<LocalisationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Pageable<LocalisationDto>>> SearchLocalisations(
                                                                [FromQuery(Name = "pageSize")] int? pageSize,
                                                                [FromQuery(Name = "page")] int? page,
                                                                [FromQuery(Name = "orderBy")] string orderBy,
                                                                [FromQuery(Name = "order")] Order? order,
                                                                [FromQuery(Name = "name")] string nameFilter)
        {
            //TODO refactor this part with the one in LeaseOfferController
            var orderAsEnum = order ?? Order.Asc;
            var orderTdoField = nameof(LocalisationDtoProperties.Name);
            if (order.HasValue && string.IsNullOrWhiteSpace(orderBy))
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    return Problem(string.Format(Resources.order_whithout_orderby, order, orderBy), null, StatusCodes.Status400BadRequest);
                }
            }

            if (!string.IsNullOrWhiteSpace(orderBy) && !OrdereableFieldsMap.TryGetValue(orderBy, out orderTdoField))
            {
                return Problem(string.Format(Resources.unknown_order_field_name, orderBy), null, StatusCodes.Status400BadRequest);
            }

            if (page.HasValue && page < 1)
            {
                return Problem(string.Format(Resources.requested_page_index_not_valid, page), null, StatusCodes.Status400BadRequest);
            }

            if (pageSize.HasValue && pageSize < 1)
            {
                return Problem(string.Format(Resources.requested_page_size_not_valid, pageSize), null, StatusCodes.Status400BadRequest);
            }

            var pageableLocalisations = await _localisationService.SearchLocalisationsAsync(pageSize ?? _functionalSettings.DefaultMaxPageSize, page ?? 1, nameFilter, orderTdoField, order ?? orderAsEnum);
            return Ok(pageableLocalisations);
        }
    }
}
