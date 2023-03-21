using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using fonenako_service;
using fonenako_service.Daos;
using fonenako_service.Dtos;
using fonenako_service.Exceptions;
using fonenako_service.Properties;
using fonenako_service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace fonenako.Controllers
{

    [ApiController]
    [Produces("application/json")]
    [Route("api/V1/Lease-offers")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

    public class LeaseOfferController : ControllerBase
    {

        private readonly ILeaseOfferService _leaseOfferService;

        private readonly FunctionalSettings _functionalSettings;

        private readonly IFilterParser _filterParser;


        private static readonly Dictionary<string, string> OrdereableFieldsMap = new()
        {
            { LeaseOfferDtoProperties.LeaseOfferID, nameof(LeaseOfferDto.LeaseOfferID) },
            { LeaseOfferDtoProperties.Surface, nameof(LeaseOfferDto.Surface) },
            { LeaseOfferDtoProperties.MonthlyRent, nameof(LeaseOfferDto.MonthlyRent) }
        };

        private static readonly Dictionary<string, Type> FilterableFieldsMap = new()
        {
            { LeaseOfferFilterFields.SurfaceMin, typeof(double) },
            { LeaseOfferFilterFields.SurfaceMax, typeof(double) },
            { LeaseOfferFilterFields.MonthlyRentMin, typeof(double) },
            { LeaseOfferFilterFields.MonthlyRentMax, typeof(double) },
            { LeaseOfferFilterFields.RoomsMin, typeof(int) },
            { LeaseOfferFilterFields.RoomsMax, typeof(int) }
        };

        public LeaseOfferController(ILeaseOfferService leaseOfferService, IOptions<FunctionalSettings> options, IFilterParser filterParser)
        {
            _leaseOfferService = leaseOfferService ?? throw new ArgumentNullException(nameof(leaseOfferService));
            _functionalSettings = options?.Value ?? throw new ArgumentException("Argument or its 'Value' is null", nameof(options));
            _filterParser = filterParser ?? throw new ArgumentNullException(nameof(filterParser));
        }

        [HttpGet(Name = "Retrieve many lease offers")]
        [ProducesResponseType(typeof(Pageable<LeaseOfferDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pageable<LeaseOfferDto>>> RetrieveManyAsync([FromQuery(Name = "pageSize")] int? pageSize,
                                                                [FromQuery(Name = "page")] int? page,
                                                                [FromQuery(Name = "orderBy")] string orderBy,
                                                                [FromQuery(Name = "order")] string order,
                                                                [FromQuery(Name = "filter")] string filter)
        {
            var orderAsEnum = Order.Desc;
            var orderTdoField = nameof(LeaseOfferDto.LeaseOfferID);
            IDictionary<string, object> filterMap = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(order))
            {
                if(string.IsNullOrWhiteSpace(orderBy))
                {
                    return Problem(string.Format(Resources.order_whithout_orderby, order, orderBy), null, (int?)HttpStatusCode.BadRequest);
                }

                try
                {
                    orderAsEnum = (Order)Enum.Parse(typeof(Order), order, true);
                }
                catch(ArgumentException)
                {
                    return Problem(string.Format(Resources.unknown_order_value, order), null, (int?)HttpStatusCode.BadRequest);
                }
            }

            if (!string.IsNullOrWhiteSpace(orderBy) && !OrdereableFieldsMap.TryGetValue(orderBy, out orderTdoField))
            {
                return Problem(string.Format(Resources.unknown_order_field_name, orderBy), null, (int?)HttpStatusCode.BadRequest);
            }

            if (page.HasValue && page < 1)
            {
                return Problem(string.Format(Resources.requested_page_index_not_valid, page), null, (int?)HttpStatusCode.BadRequest);
            }

            if (pageSize.HasValue && pageSize < 1)
            {
                return Problem(string.Format(Resources.requested_page_size_not_valid, pageSize), null, (int?) HttpStatusCode.BadRequest);
            }

            var decodedFilter = HttpUtility.UrlDecode(filter ?? "");
            if (!string.IsNullOrWhiteSpace(decodedFilter))
            {
                try
                {
                    filterMap = _filterParser.ParseFilter(decodedFilter, FilterableFieldsMap);
                }
                catch(UnknownFilterFieldException ex)
                {
                    return Problem(string.Format(Resources.unknown_order_field_name, ex.FieldName), null, (int?)HttpStatusCode.BadRequest);
                }
                catch(DuplicateFilterFieldException ex)
                {
                    return Problem(string.Format(Resources.duplicate_filter_field, ex.FieldName), null, (int?)HttpStatusCode.BadRequest);
                }
                catch(InvalidFilterFieldValueException ex)
                {
                    return Problem(string.Format(Resources.invalid_filter_field_value, ex.FieldName, ex.InvalidValue), null, (int?)HttpStatusCode.BadRequest);
                }
            }


            var pageable = await _leaseOfferService.RetrieveLeaseOffersAsync(pageSize ?? _functionalSettings.DefaultMaxPageSize, page ?? 1, filterMap, orderTdoField, orderAsEnum);
            
            return Ok(pageable);
        }

        [HttpGet("{leaseOfferId}", Name = "Retrieve lease offer by ID")]
        [ProducesResponseType(typeof(LeaseOfferDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async  Task<ActionResult<LeaseOfferDto>> RetrieveSingle([FromRoute(Name = "leaseOfferId")] int leaseOfferId)
        {
            if (leaseOfferId < 1)
            {
                return Problem(string.Format(Resources.invalid_lease_offer_id, leaseOfferId), null, (int?)HttpStatusCode.BadRequest);
            }

            var leaseOfferDto = await _leaseOfferService.FindLeaseOfferByIdAsync(leaseOfferId);
            if(leaseOfferDto == null)
            {
                return NotFound();
            }

            return Ok(leaseOfferDto);
        }
    }
}