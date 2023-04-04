using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using fonenako_service;
using fonenako_service.Daos;
using fonenako_service.Dtos;
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]

    public class LeaseOfferController : ControllerBase
    {

        private readonly ILeaseOfferService _leaseOfferService;

        private readonly FunctionalSettings _functionalSettings;


        private static readonly Dictionary<string, string> OrdereableFieldsMap = new()
        {
            { LeaseOfferDtoProperties.LeaseOfferID, nameof(LeaseOfferDto.LeaseOfferID) },
            { LeaseOfferDtoProperties.Surface, nameof(LeaseOfferDto.Surface) },
            { LeaseOfferDtoProperties.MonthlyRent, nameof(LeaseOfferDto.MonthlyRent) },
            { LeaseOfferDtoProperties.CreationDate, nameof(LeaseOfferDto.CreationDate) }
        };

        public LeaseOfferController(ILeaseOfferService leaseOfferService, IOptions<FunctionalSettings> options)
        {
            _leaseOfferService = leaseOfferService ?? throw new ArgumentNullException(nameof(leaseOfferService));
            _functionalSettings = options?.Value ?? throw new ArgumentException("Argument or its 'Value' is null", nameof(options));
        }

        [HttpGet(Name = "Retrieve many lease offers")]
        [ProducesResponseType(typeof(Pageable<LeaseOfferDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Pageable<LeaseOfferDto>>> RetrieveManyAsync([FromQuery(Name = "pageSize")] int? pageSize,
                                                                [FromQuery(Name = "page")] int? page,
                                                                [FromQuery(Name = "orderBy")] string orderBy,
                                                                [FromQuery(Name = "order")] Order? order,
                                                                [FromQueryAsJson(Name = "filter")] LeaseOfferFilter filter)
        {
            var orderAsEnum = order ?? Order.Desc;
            var orderTdoField = nameof(LeaseOfferDto.LeaseOfferID);
            if (order.HasValue && string.IsNullOrWhiteSpace(orderBy))
            {
                if(string.IsNullOrWhiteSpace(orderBy))
                {
                    return Problem(string.Format(Resources.order_whithout_orderby, order, orderBy), null, (int?)HttpStatusCode.BadRequest);
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

            var pageable = await _leaseOfferService.RetrieveLeaseOffersAsync(pageSize ?? _functionalSettings.DefaultMaxPageSize, page ?? 1, filter ?? LeaseOfferFilter.Default, orderTdoField, orderAsEnum);
            
            return Ok(pageable);
        }

        [HttpGet("{leaseOfferId}", Name = "Retrieve lease offer by Id")]
        [ProducesResponseType(typeof(LeaseOfferDto), StatusCodes.Status200OK)]
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