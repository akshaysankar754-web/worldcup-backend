using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpPost("vote")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Vote(VoteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var result = await _pollService.VoteAsync(userId, dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("open")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OpenPoll()
        {
            try
            {
                var result = await _pollService.OpenPollAsync();
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("close")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ClosePoll()
        {
            try
            {
                var result = await _pollService.ClosePollAsync();
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPollStatus()
        {
            var isOpen = await _pollService.IsPollOpenAsync();
            return Ok(new { pollOpen = isOpen });
        }
    }
}
