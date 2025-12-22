using EasyWorkout.Api.Auth;
using EasyWorkout.Api.Mapping;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyWorkout.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Users.Get)]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");

            var user = await _userService.GetByIdAsync(userId!.Value, token);
            if (user is null) return NotFound($"User with id {userId!.Value} not found.");

            return Ok(user.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Users.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");
            if (userId != id) return BadRequest("User id does not match requested id.");

            var user = await _userService.UpdateAsync(id, request, token);
            if (user is null) return NotFound();
            return Ok(user.MapToResponse());
        }
    }
}
