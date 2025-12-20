using EasyWorkout.Api.Auth;
using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
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
        private readonly WorkoutsContext _workoutsContext;
        private readonly ILogger _logger;
        public WorkoutController(WorkoutsContext workoutsContext, ILogger<WorkoutController> logger)
        {
            _workoutsContext = workoutsContext;
            _logger = logger;
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Workouts.Create)]
        public async Task<IActionResult> Create(CreateWorkoutRequest request)
        {
            var userId = HttpContext.GetUserId();

            if (_workoutsContext.Workouts.Any(w => w.Name == request.Name && w.AddedByUserId == userId))
                return BadRequest($"Workout with the name {request.Name} already exists for user {userId}.");

            Workout workout = new Workout()
            {
                Id = Guid.NewGuid(),
                AddedByUserId = userId!.Value,
                AddedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Name = request.Name,
                Notes = request.Notes,
                Exercises = []
            };

            _workoutsContext.Workouts.Add(workout);
            await _workoutsContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = workout.Id }, workout);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Workouts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_workoutsContext.Workouts);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpGet(Endpoints.Workouts.Get)]
        public async Task<IActionResult> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPut(Endpoints.Workouts.Update)]
        public async Task<IActionResult> Update(Guid id) // will pass request object from body
        {
            throw new NotImplementedException();
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpDelete(Endpoints.Workouts.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}
