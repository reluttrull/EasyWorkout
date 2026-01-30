using EasyWorkout.Api.Auth;
using EasyWorkout.Api.Mapping;
using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Contracts.Responses;
using EasyWorkout.Identity.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using static EasyWorkout.Api.Endpoints;

namespace EasyWorkout.Api.Controllers
{
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        private readonly ILogger _logger;
        public WorkoutController(IWorkoutService workoutService, ILogger<WorkoutController> logger)
        {
            _workoutService = workoutService;
            _logger = logger;
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Workouts.Create)]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var workout = request.MapToWorkout(userId!.Value);
            await _workoutService.CreateAsync(workout, token);

            _logger.LogInformation("User {userId} created new workout {workoutId}", userId, workout.Id);
            return CreatedAtAction(nameof(Get), new { id = workout.Id }, workout);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Workouts.GetAllForUser)]
        public async Task<IActionResult> GetAllForUser(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var detailedWorkoutsForUser = await _workoutService.GetAllDetailedForUserAsync(userId!.Value, token);
            var workoutResponsesForUser = detailedWorkoutsForUser.Select(w => w.Workout.MapToResponse(w.LastCompletedDate));

            return Ok(workoutResponsesForUser.AsEnumerable());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Workouts.Get)]
        public async Task<IActionResult> Get(Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _workoutService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Workout with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Workout with id {id} does not belong to user {userId}.");
            }

            var detailedWorkout = await _workoutService.GetByIdDetailedAsync(id, token);
            if (detailedWorkout is null) return NotFound($"Workout with id {id} not found.");

            return Ok(detailedWorkout.Workout.MapToResponse(detailedWorkout.LastCompletedDate));
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Workouts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWorkoutRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _workoutService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Workout with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Workout with id {id} does not belong to user {userId}.");
            }

            var workout = await _workoutService.UpdateAsync(id, request, token);
            if (workout is null) return NotFound();
            _logger.LogInformation("User {userId} updated workout {workoutId}", userId, workout.Id);
            return Ok(workout.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Workouts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _workoutService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Workout with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Workout with id {id} does not belong to user {userId}.");
            }

            var success = await _workoutService.DeleteAsync(id, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} deleted workout {workoutId}", userId, id);
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Workouts.DeleteAll)]
        public async Task<IActionResult> DeleteAll(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var success = await _workoutService.DeleteAllAsync(userId!.Value, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} deleted all workouts", userId);
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Workouts.AddExercise)]
        public async Task<IActionResult> AddExercise([FromRoute] Guid id, Guid exerciseId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _workoutService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Workout with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Workout with id {id} does not belong to user {userId}.");
            }

            var success = await _workoutService.AddExerciseAsync(id, exerciseId, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} added exercise {exerciseId} to workout {workoutId}", userId, exerciseId, id);
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Workouts.RemoveExercise)]
        public async Task<IActionResult> RemoveExercise([FromRoute] Guid id, Guid exerciseId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _workoutService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Workout with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Workout with id {id} does not belong to user {userId}.");
            }

            var success = await _workoutService.RemoveExerciseAsync(id, exerciseId, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} removed exercise {exerciseId} from workout {workoutId}", userId, exerciseId, id);
            return Ok();
        }
    }
}
