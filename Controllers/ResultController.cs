using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpPost("reveal")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RevealResult()
        {
            try
            {
                var result = await _resultService.RevealResultAsync();
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("hide")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HideResult()
        {
            try
            {
                var result = await _resultService.HideResultAsync();
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetResults()
        {
            try
            {
                bool isAdmin = User.IsInRole("Admin");
                var results = await _resultService.GetResultsAsync(isAdmin);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("status")]
        public async Task<IActionResult> GetResultStatus()
        {
            var isVisible = await _resultService.IsResultVisibleAsync();
            return Ok(new { resultVisible = isVisible });
        }
    }
}
