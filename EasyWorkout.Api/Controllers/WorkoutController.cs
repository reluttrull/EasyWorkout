using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
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

        [HttpPost(Endpoints.Workouts.Create)]
        public async Task<IActionResult> Create() // will pass request object from body
        {
            throw new NotImplementedException();
        }

        [HttpGet(Endpoints.Workouts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_workoutsContext.Workouts);
        }

        [HttpGet(Endpoints.Workouts.Get)]
        public async Task<IActionResult> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut(Endpoints.Workouts.Update)]
        public async Task<IActionResult> Update(Guid id) // will pass request object from body
        {
            throw new NotImplementedException();
        }

        [HttpDelete(Endpoints.Workouts.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}
