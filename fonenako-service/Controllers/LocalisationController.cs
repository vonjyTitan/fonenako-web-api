
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fonenako_service.Controllers.Validator;
using fonenako_service.Daos;
using fonenako_service.Dtos;
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

        private readonly IPageableEndPointInputValidator _pageableEndPointInputValidator;

        private static readonly Dictionary<string, string> SortableFieldsMap = new()
        {
            { LocalisationDtoProperties.Name, nameof(LocalisationDto.Name) },
            { LocalisationDtoProperties.Type, nameof(LocalisationDto.Type) },
        };

        public LocalisationController(ILocalisationService localisationService, IOptions<FunctionalSettings> options, IEndPointInputValidatorFactory endPointInputValidatorFactory)
        {
            _localisationService = localisationService ?? throw new ArgumentNullException(nameof(localisationService));
            _functionalSettings = options?.Value ?? throw new ArgumentException("Argument or its 'Value' is null", nameof(options));
            if (endPointInputValidatorFactory == null) throw new ArgumentNullException(nameof(endPointInputValidatorFactory));
            _pageableEndPointInputValidator = endPointInputValidatorFactory.CreatePageableEndPointInputValidator(SortableFieldsMap.Keys.ToHashSet()) ?? throw new InvalidOperationException("No IPageableEndPointInputValidator found");
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
            var pageableParams = new PageableRequestParam
            {
                PageSize = pageSize,
                Order = order,
                Page = page,
                OrderBy = orderBy
            };
            if (!_pageableEndPointInputValidator.IsValide(pageableParams, out var firstError))
            {
                return Problem(firstError.Message, null, StatusCodes.Status400BadRequest, null, firstError.ErrorCode);
            }

            var orderTdoField = nameof(LocalisationDtoProperties.Name);
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderTdoField = SortableFieldsMap[orderBy];
            }

            var pageableLocalisations = await _localisationService.SearchLocalisationsAsync(pageSize ?? _functionalSettings.DefaultMaxPageSize, page ?? 1, nameFilter, orderTdoField, order ?? Order.Asc);
            return Ok(pageableLocalisations);
        }
    }
}
