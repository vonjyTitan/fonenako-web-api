using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fonenako_service;
using fonenako_service.Controllers;
using fonenako_service.Controllers.Validator;
using fonenako_service.Controllers.Validators;
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

        private readonly IPageableEndPointInputValidator _pageableEndPointInputValidator;

        private static readonly Dictionary<string, string> SortableFieldsMap = new()
        {
            { LeaseOfferDtoProperties.LeaseOfferID, nameof(LeaseOfferDto.LeaseOfferID) },
            { LeaseOfferDtoProperties.Surface, nameof(LeaseOfferDto.Surface) },
            { LeaseOfferDtoProperties.MonthlyRent, nameof(LeaseOfferDto.MonthlyRent) },
            { LeaseOfferDtoProperties.CreationDate, nameof(LeaseOfferDto.CreationDate) }
        };

        public LeaseOfferController(ILeaseOfferService leaseOfferService, IOptions<FunctionalSettings> options, IEndPointInputValidatorFactory endPointInputValidatorFactory)
        {
            _leaseOfferService = leaseOfferService ?? throw new ArgumentNullException(nameof(leaseOfferService));
            _functionalSettings = options?.Value ?? throw new ArgumentException("Argument or its 'Value' is null", nameof(options));
            if (endPointInputValidatorFactory == null) throw new ArgumentNullException(nameof(endPointInputValidatorFactory));
            _pageableEndPointInputValidator = endPointInputValidatorFactory.CreatePageableEndPointInputValidator(SortableFieldsMap.Keys.ToHashSet()) ?? throw new InvalidOperationException("No IPageableEndPointInputValidator found");
        }

        [HttpGet(Name = "Retrieve many lease offers")]
        [ProducesResponseType(typeof(Pageable<LeaseOfferDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Pageable<LeaseOfferDto>>> RetrieveManyAsync(
                                                                [FromQuery(Name = "pageSize")] int? pageSize,
                                                                [FromQuery(Name = "page")] int? page,
                                                                [FromQuery(Name = "orderBy")] string orderBy,
                                                                [FromQuery(Name = "order")] Order? order,
                                                                [FromQueryAsJson(Name = "filter")] LeaseOfferFilter filter)
        {
            var pageableParams = new PageableRequestParam
            {
                PageSize = pageSize,
                Order = order,
                Page = page,
                OrderBy = orderBy
            };
            if(!_pageableEndPointInputValidator.IsValide(pageableParams, out var firstError))
            {
                return Problem(firstError.Message, null, StatusCodes.Status400BadRequest, null, firstError.ErrorCode);
            }
            var orderTdoField = nameof(LeaseOfferDto.LeaseOfferID);
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderTdoField = SortableFieldsMap[orderBy];
            }

            var pageable = await _leaseOfferService.RetrieveLeaseOffersAsync(pageSize ?? _functionalSettings.DefaultMaxPageSize, page ?? 1, filter ?? LeaseOfferFilter.Default, orderTdoField, order ?? Order.Desc);
            return Ok(pageable);
        }

        [HttpGet("{leaseOfferId}", Name = "Retrieve lease offer details by Id")]
        [ProducesResponseType(typeof(LeaseOfferDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async  Task<ActionResult<LeaseOfferDto>> RetrieveSingle([FromRoute(Name = "leaseOfferId")] int leaseOfferId)
        {
            if (leaseOfferId < 1)
            {
                return Problem(string.Format(Resources.invalid_resource_id, leaseOfferId), null, StatusCodes.Status400BadRequest, null, ErrorCode.InvalidResourceId);
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