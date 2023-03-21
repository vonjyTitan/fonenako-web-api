using System;
using System.Threading.Tasks;
using fonenako_service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fonenako_service.Controllers
{
    [ApiController]
    [Route("api/V1")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class SystemController : ControllerBase
    {
        private readonly ILeaseOfferService _leaseOfferService;

        public SystemController(ILeaseOfferService leaseOfferService)
        {
            _leaseOfferService = leaseOfferService ?? throw new ArgumentNullException(nameof(leaseOfferService));
        }

        [HttpPost]
        [Route("Lease-offers-initializer")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> InitAsync()
        {
            await _leaseOfferService.InitAsync();
            return Accepted();
        }
    }
}
