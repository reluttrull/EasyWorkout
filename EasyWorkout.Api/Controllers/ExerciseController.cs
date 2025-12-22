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
            if (userId is null) return BadRequest("User not found.");

            var exercise = request.MapToExercise(userId!.Value);
            await _exerciseService.CreateAsync(exercise, token);

            return CreatedAtAction(nameof(Get), new { id = exercise.Id }, exercise);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Exercises.GetAllForUser)]
        public async Task<IActionResult> GetAllForUser(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");

            var exercisesForUser = await _exerciseService.GetAllForUserAsync(userId!.Value, token);
            var exerciseResponsesForUser = exercisesForUser.Select(e => e.MapToResponse());

            return Ok(exerciseResponsesForUser);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Exercises.Get)]
        public async Task<IActionResult> Get(Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser) return BadRequest($"Exercise with id {id} does not belong to user {userId}.");

            var exercise = await _exerciseService.GetByIdAsync(id, token);
            if (exercise is null) return NotFound($"Exercise with id {id} not found.");

            return Ok(exercise.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Exercises.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateExerciseRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser) return BadRequest($"Exercise with id {id} does not belong to user {userId}.");

            var exercise = await _exerciseService.UpdateAsync(id, request, token);
            if (exercise is null) return NotFound();
            return Ok(exercise.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Exercises.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser) return BadRequest($"Exercise with id {id} does not belong to user {userId}.");

            var success = await _exerciseService.DeleteAsync(id, token);
            if (!success) return NotFound();
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Exercises.CreateSet)]
        public async Task<IActionResult> CreateSet([FromRoute] Guid id, [FromBody] CreateSetRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser) return BadRequest($"Exercise with id {id} does not belong to user {userId}.");

            var exerciseSet = request.MapToExerciseSet(id);

            var success = await _exerciseService.CreateSetAsync(id, exerciseSet, token);
            if (!success) return NotFound();
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Exercises.DeleteSet)]
        public async Task<IActionResult> DeleteSet([FromRoute] Guid id, Guid exerciseSetId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");
            var belongsToUser = await _exerciseService.BelongsToUserAsync(id, userId!.Value, token);
            if (!belongsToUser) return BadRequest($"Exercise with id {id} does not belong to user {userId}.");

            var success = await _exerciseService.DeleteSetAsync(id, exerciseSetId, token);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
