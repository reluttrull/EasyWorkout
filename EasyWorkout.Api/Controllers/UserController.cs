using EasyWorkout.Api.Auth;
using EasyWorkout.Api.Mapping;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api;
using EasyWorkout.Identity.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using static EasyWorkout.Api.Endpoints;

namespace EasyWorkout.Api.Controllers
{
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Users.Get)]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var user = await _userService.GetByIdAsync(userId!.Value, token);
            if (user is null) return NotFound($"User with id {userId!.Value} not found.");

            return Ok(user.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Users.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var user = await _userService.UpdateAsync(userId!.Value, request, token);
            if (user is null) return NotFound();
            _logger.LogInformation("User with id {id} updated account info.", userId);
            return Ok(user.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Users.ChangeEmail)]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var user = await _userService.ChangeEmailAsync(userId!.Value, request, token);
            if (user is null) return NotFound();
            _logger.LogInformation("User with id {id} changed email to {email}.", userId, request.Email);
            return Ok(user.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Users.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var user = await _userService.ChangePasswordAsync(userId!.Value, request, token);
            if (user is null) return NotFound();
            _logger.LogInformation("User with id {id} changed password.", userId);
            return Ok(user.MapToResponse());
        }
    }
}