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
        private readonly IWorkoutService _workoutService;
        private readonly IExerciseService _exerciseService;
        private readonly ILogger _logger;

        public CompletedWorkoutController(
            ICompletedWorkoutService completedWorkoutService,
            IWorkoutService workoutService,
            IExerciseService exerciseService,
            ILogger<CompletedWorkoutController> logger)
        {
            _completedWorkoutService = completedWorkoutService;
            _workoutService = workoutService;
            _exerciseService = exerciseService;
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
            // copy values from original workout
            if (completedWorkout.WorkoutId is not null)
            {
                var basedOnWorkout = await _workoutService.GetByIdAsync(completedWorkout.WorkoutId!.Value);
                if (basedOnWorkout is null) return NotFound();
                completedWorkout.FallbackName = basedOnWorkout.Name;
            }
            foreach (CompletedExercise completedExercise in completedWorkout.CompletedExercises)
            {
                if (completedExercise.ExerciseId is not null)
                {
                    var basedOnExercise = await _exerciseService.GetByIdAsync(completedExercise.ExerciseId!.Value);
                    if (basedOnExercise is null) return NotFound();
                    completedExercise.FallbackName = basedOnExercise.Name;
                }
                foreach (CompletedExerciseSet completedExerciseSet in completedExercise.CompletedExerciseSets)
                {
                    if (completedExerciseSet.ExerciseSetId is not null)
                    {
                        var basedOnSet = await _exerciseService.GetSetByIdAsync(completedExerciseSet.ExerciseSetId!.Value);
                        if (basedOnSet is null) return NotFound();
                        completedExerciseSet.DurationUnit = basedOnSet.DurationUnit;
                        completedExerciseSet.WeightUnit = basedOnSet.WeightUnit;
                        completedExerciseSet.DistanceUnit = basedOnSet.DistanceUnit;
                        completedExerciseSet.GoalDuration = basedOnSet.Duration;
                        completedExerciseSet.GoalWeight = basedOnSet.Weight;
                        completedExerciseSet.GoalDistance = basedOnSet.Distance;
                        completedExerciseSet.GoalReps = basedOnSet.Reps;
                    }
                }
            }
            await _completedWorkoutService.CreateAsync(completedWorkout, token);

            _logger.LogInformation("User {userId} finished workout {workoutId} with completed workout id {completedWorkoutId}.",
                userId, completedWorkout.WorkoutId, completedWorkout.Id);
            return CreatedAtAction(nameof(Get), new { id = completedWorkout.Id }, completedWorkout.MapToResponse());
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

            var detailedCompletedWorkout = await _completedWorkoutService.GetByIdAsync(id, token);
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

            var detailedCompletedWorkoutsForUser = await _completedWorkoutService.GetAllForUserAsync(userId!.Value, token);
            var completedWorkoutResponsesForUser = detailedCompletedWorkoutsForUser.Select(w => w.MapToResponse());

            return Ok(completedWorkoutResponsesForUser);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.CompletedWorkouts.GetLastCompletedDate)]
        public async Task<IActionResult> GetLastCompletedDate(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var lastCompletedDate = await _completedWorkoutService.GetLastCompletedDateOrDefault(userId!.Value, token);
            return Ok(lastCompletedDate);
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
            _logger.LogInformation("User {userId} updated completed workout {workoutId}", userId, completedWorkout.Id);
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

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.CompletedWorkouts.DeleteAll)]
        public async Task<IActionResult> DeleteAll(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var success = await _completedWorkoutService.DeleteAllAsync(userId!.Value, token);
            if (!success) return NotFound();
            _logger.LogInformation("User {userId} deleted all completed workouts", userId);
            return Ok();
        }
    }
}
