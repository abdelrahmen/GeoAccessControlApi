using GeoAccessControlAPI.Models.RequestModels;
using GeoAccessControlAPI.Services;
using GeoAccessControlAPI.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoAccessControlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IBlockService _iPBlockService;

        public LogsController(IBlockService iPBlockService)
        {
            _iPBlockService = iPBlockService;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] PaginationRequestModel request)
        {
            var response = _iPBlockService.GetBlockedLogs(request);
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
