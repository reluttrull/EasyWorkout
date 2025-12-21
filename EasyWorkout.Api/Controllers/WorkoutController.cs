using EasyWorkout.Api.Auth;
using EasyWorkout.Api.Mapping;
using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if (userId is null) return BadRequest("User not found.");

            var workout = request.MapToWorkout(userId!.Value);
            await _workoutService.CreateAsync(workout, token);

            return CreatedAtAction(nameof(Get), new { id = workout.Id }, workout);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Workouts.GetAllForUser)]
        public async Task<IActionResult> GetAllForUser(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");

            var workoutsForUser = await _workoutService.GetAllForUserAsync(userId!.Value, token);
            var workoutResponsesForUser = workoutsForUser.Select(w => w.MapToResponse());

            return Ok(workoutResponsesForUser);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Workouts.Get)]
        public async Task<IActionResult> Get(Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");

            var workout = await _workoutService.GetByIdAsync(id, token);
            if (workout is null) return NotFound($"Workout with id {id} not found.");
            if (workout.AddedByUserId != userId) return BadRequest($"Workout with id {id} does not belong to user with id {userId}.");

            return Ok(workout.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Workouts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWorkoutRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");

            var workout = await _workoutService.UpdateAsync(id, request, userId!.Value, token);
            if (workout is null) return NotFound();
            return Ok(workout.MapToResponse());
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Workouts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");

            var success = await _workoutService.DeleteAsync(id, userId!.Value, token);
            if (!success) return NotFound();
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Workouts.AddExercise)]
        public async Task<IActionResult> AddExercise([FromRoute] Guid id, Guid exerciseId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");

            var workout = await _workoutService.GetByIdAsync(id, token);
            if (workout is null) return NotFound($"Workout with id {id} not found.");
            if (workout.AddedByUserId != userId) return BadRequest($"Workout with id {id} does not belong to user with id {userId}.");

            var success = await _workoutService.AddExerciseAsync(id, exerciseId, userId!.Value, token);
            if (!success) return NotFound();
            return Ok();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Workouts.RemoveExercise)]
        public async Task<IActionResult> RemoveExercise([FromRoute] Guid id, Guid exerciseId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null) return BadRequest("User not found.");

            var workout = await _workoutService.GetByIdAsync(id, token);
            if (workout is null) return NotFound($"Workout with id {id} not found.");
            if (workout.AddedByUserId != userId) return BadRequest($"Workout with id {id} does not belong to user with id {userId}.");

            var success = await _workoutService.RemoveExerciseAsync(id, exerciseId, userId!.Value, token);
            if (!success) return NotFound();
            return Ok();
        }

    }
}
