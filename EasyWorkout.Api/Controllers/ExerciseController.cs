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
using static EasyWorkout.Api.Endpoints;

namespace EasyWorkout.Api.Controllers
{
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly ILogger _logger;
        public ExerciseController(IExerciseService exerciseService, ILogger<WorkoutController> logger)
        {
            _exerciseService = exerciseService;
            _logger = logger;
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Exercises.Create)]
        public async Task<IActionResult> Create([FromBody] CreateExerciseRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var exercise = request.MapToExercise(userId!.Value);
            await _exerciseService.CreateAsync(exercise, token);

            _logger.LogInformation("User {userId} created new exercise {exerciseId}", userId, exercise.Id);
            return CreatedAtAction(nameof(Get), new { id = exercise.Id }, exercise);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Exercises.GetAllForUser)]
        public async Task<IActionResult> GetAllForUser(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var exercisesForUser = await _exerciseService.GetAllForUserAsync(userId!.Value, token);
            var exerciseResponsesForUser = exercisesForUser.Select(e => e.MapToResponse());

            return Ok(exerciseResponsesForUser);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Exercises.Get)]
        public async Task<IActionResult> Get(Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Exercise with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Exercise with id {id} does not belong to user {userId}.");
            }

            var exercise = await _exerciseService.GetByIdAsync(id, token);
            if (exercise is null) return NotFound($"Exercise with id {id} not found.");

            return Ok(exercise.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Exercises.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateExerciseRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Exercise with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Exercise with id {id} does not belong to user {userId}.");
            }

            var exercise = await _exerciseService.UpdateAsync(id, request, token);
            if (exercise is null) return NotFound();
            _logger.LogInformation("User {userId} updated exercise {exerciseId}", userId, exercise.Id);
            return Ok(exercise.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Exercises.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Exercise with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Exercise with id {id} does not belong to user {userId}.");
            }

            var success = await _exerciseService.DeleteAsync(id, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} deleted exercise {exerciseId}", userId, id);
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Exercises.DeleteAll)]
        public async Task<IActionResult> DeleteAll(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var success = await _exerciseService.DeleteAllAsync(userId!.Value, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} deleted all exercises", userId);
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Exercises.CreateSet)]
        public async Task<IActionResult> CreateSet([FromRoute] Guid id, [FromBody] CreateSetRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Exercise with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Exercise with id {id} does not belong to user {userId}.");
            }

            var setNumber = await _exerciseService.GetNextSetIndexAsync(id, token);

            var exerciseSet = request.MapToExerciseSet(id, setNumber);

            var success = await _exerciseService.CreateSetAsync(id, exerciseSet, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} created set {setId} for exercise {exerciseId}", userId, exerciseSet.Id, id);
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Exercises.DeleteSet)]
        public async Task<IActionResult> DeleteSet([FromRoute] Guid id, Guid exerciseSetId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser)
            {
                _logger.LogWarning("Exercise with id {id} does not belong to user {userId}.", id, userId);
                return BadRequest($"Exercise with id {id} does not belong to user {userId}.");
            }

            var success = await _exerciseService.DeleteSetAsync(id, exerciseSetId, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} deleted set {setId} from exercise {exerciseId}", userId, exerciseSetId, id);
            return Ok();
        }
    }
}
