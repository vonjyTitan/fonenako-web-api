
using System;
using System.Collections.Generic;
using System.Net;
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
    [Route("api/V1/Cities")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        private readonly FunctionalSettings _functionalSettings;

        private static readonly Dictionary<string, string> SortableFieldsMap = new()
        {
            { "name", nameof(CityDto.Name) },
        };

        public CityController(ICityService cityService, IOptions<FunctionalSettings> options)
        {
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _functionalSettings = options?.Value ?? throw new ArgumentException("Argument or its 'Value' is null", nameof(options));
        }

        [HttpGet(Name ="Retrieve cities list")]
        [ProducesResponseType(typeof(Pageable<CityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Pageable<CityDto>>> RetrieveCitiesAsync(
                                                                            [FromQuery(Name = "pageSize")] int? pageSize,
                                                                [FromQuery(Name = "page")] int? page,
                                                                [FromQuery(Name = "orderBy")] string orderBy,
                                                                [FromQuery(Name = "order")] Order? order,
                                                                [FromQueryAsJson(Name = "filter")] CityFilter filter)
        {
            var orderAsEnum = order ?? Order.Desc;
            var orderTdoField = nameof(CityDto.Name);
            if (order.HasValue && string.IsNullOrWhiteSpace(orderBy))
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    return Problem(string.Format(Resources.order_whithout_orderby, order, orderBy), null, StatusCodes.Status400BadRequest);
                }
            }

            if (!string.IsNullOrWhiteSpace(orderBy) && !SortableFieldsMap.TryGetValue(orderBy, out orderTdoField))
            {
                return Problem(string.Format(Resources.unknown_order_field_name, orderBy), null, StatusCodes.Status400BadRequest);
            }

            if (page.HasValue && page < 1)
            {
                return Problem(string.Format(Resources.requested_page_index_not_valid, page), null, StatusCodes.Status400BadRequest);
            }

            if (pageSize.HasValue && pageSize < 1)
            {
                return Problem(string.Format(Resources.requested_page_size_not_valid, pageSize), null, (int?)HttpStatusCode.BadRequest);
            }

            await _cityService.RetrieveCitiesAsync(pageSize ?? _functionalSettings.DefaultMaxPageSize, page ?? 1, filter ?? CityFilter.Default, orderTdoField, orderAsEnum);

            return NotFound();
        }
    }
}
