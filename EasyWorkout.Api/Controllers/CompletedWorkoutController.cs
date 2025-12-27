using EasyWorkout.Api.Auth;
using EasyWorkout.Api.Mapping;
using EasyWorkout.Application.Model;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EasyWorkout.Api.Controllers
{
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class CompletedWorkoutController : ControllerBase
    {
        private readonly ICompletedWorkoutService _completedWorkoutService;
        private readonly ILogger _logger;

        public CompletedWorkoutController(ICompletedWorkoutService completedWorkoutService, ILogger<CompletedWorkoutController> logger)
        {
            _completedWorkoutService = completedWorkoutService;
            _logger = logger;
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.CompletedWorkouts.Create)]
        public async Task<IActionResult> Create([FromBody] FinishWorkoutRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var completedWorkout = request.MapToCompletedWorkout(userId!.Value);
            await _completedWorkoutService.CreateAsync(completedWorkout, token);

            _logger.LogInformation("User {userId} finished workout {workoutId} with completed workout id {completedWorkoutId}.", 
                userId, completedWorkout.WorkoutId, completedWorkout.Id);
            return CreatedAtAction(nameof(Get), new { id = completedWorkout.Id }, completedWorkout);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.CompletedWorkouts.Get)]
        public async Task<IActionResult> Get(Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _completedWorkoutService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Completed workout with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Completed workout with id {id} does not belong to user {userId}.");
            }

            var detailedCompletedWorkout = await _completedWorkoutService.GetByIdDetailedAsync(id, token);
            if (detailedCompletedWorkout is null) return NotFound($"Completed workout with id {id} not found.");

            return Ok(detailedCompletedWorkout.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.CompletedWorkouts.GetAllForUser)]
        public async Task<IActionResult> GetAllForUser(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var detailedCompletedWorkoutsForUser = await _completedWorkoutService.GetAllDetailedForUserAsync(userId!.Value, token);
            var completedWorkoutResponsesForUser = detailedCompletedWorkoutsForUser.Select(w => w.MapToResponse());

            return Ok(completedWorkoutResponsesForUser);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.CompletedWorkouts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCompletedWorkoutRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _completedWorkoutService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Completed workout with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Completed workout with id {id} does not belong to user {userId}.");
            }

            var completedWorkout = await _completedWorkoutService.UpdateAsync(id, request, token);
            if (completedWorkout is null) return NotFound();
            _logger.LogInformation("User {userId} updated completed workout {workoutId}", userId, completedWorkout.CompletedWorkout.Id);
            return Ok(completedWorkout.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.CompletedWorkouts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _completedWorkoutService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Completed workout with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Completed workout with id {id} does not belong to user {userId}.");
            }

            var success = await _completedWorkoutService.DeleteAsync(id, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} deleted completed workout {workoutId}", userId, id);
            return Ok();
        }
    }
}
