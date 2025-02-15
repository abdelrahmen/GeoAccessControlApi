using GeoAccessControlAPI.Helpers;
using GeoAccessControlAPI.Models.RequestModels;
using GeoAccessControlAPI.Models.ResponseModels;
using GeoAccessControlAPI.Services;
using GeoAccessControlAPI.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoAccessControlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IBlockService _ipBlockService;

        public CountriesController(IBlockService ipBlockService)
        {
            _ipBlockService = ipBlockService;
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] string CountryCode)
        {
            var result = _ipBlockService.BlockCountry(CountryCode);

            if (result.Success)
                return Ok(result);

            return Conflict(result);
        }

        [HttpPost("temporal-block")]
        public IActionResult TemporarilyBlockCountry([FromBody] TemporalBlockRequest request)
        {
            if (!CountryCodeHelper.IsValidCountryCode(request.CountryCode))
                return BadRequest(ResponseModel<string>.Error("Invalid country code."));

            if (request.DurationMinutes is < 1 or > 1440)
                return BadRequest(ResponseModel<string>.Error("Duration must be between 1 and 1440 minutes (24 hours)."));

            if (_ipBlockService.TryBlockCountry(request.CountryCode, request.DurationMinutes, out var message))
                return Ok(ResponseModel<string>.SuccessResponse(message));

            return Conflict(ResponseModel<string>.Error(message));
        }

        [HttpDelete("block/{CountryCode}")]
        public IActionResult UnblockCountry(string CountryCode)
        {
            if (!CountryCodeHelper.IsValidCountryCode(CountryCode))
                return BadRequest(ResponseModel<string>.Error("Invalid country code."));

            var response = _ipBlockService.UnblockCountry(CountryCode);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] PaginationRequestModel request)
        {
            var response = _ipBlockService.GetBlockedCountries(request);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
