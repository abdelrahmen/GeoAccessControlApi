using GeoAccessControlAPI.Services;
using GeoAccessControlAPI.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoAccessControlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPController : ControllerBase
    {
        private readonly IBlockService _ipBlockService;
        private readonly ILookupService _ipLookupService;

        public IPController(IBlockService blockedCountryService, ILookupService ipLookupService)
        {
            _ipBlockService = blockedCountryService;
            _ipLookupService = ipLookupService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> FindMyCountry([FromQuery] string ipAddress = null)
        {
            var response = await _ipLookupService.GetCountryByIpAsync(ipAddress);
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlockedIP()
        {
            var response = await _ipBlockService.IsBlocked();
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
