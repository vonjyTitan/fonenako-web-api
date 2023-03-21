
using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fonenako_service.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorHandlerController : ControllerBase
    {
        private readonly ILogger<ErrorHandlerController> _logger;

        public ErrorHandlerController(ILogger<ErrorHandlerController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("/Error")]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            _logger.LogError(exceptionHandlerFeature.Error, string.Empty);
            return Problem("An internal server error occured, please contact IT Support");
        }
    }
}
