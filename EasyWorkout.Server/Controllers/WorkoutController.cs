using EasyWorkout.Application.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyWorkout.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly ILogger _logger;
        public WorkoutController(ILogger<WorkoutController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            Guid fakeUserId = Guid.NewGuid();
            List<Workout> mockWorkouts = new List<Workout>()
            {
                new Workout()
                {
                    WorkoutId = Guid.NewGuid(),
                    AddedByUserId = fakeUserId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Arm day",
                    Exercises = new List<Exercise>()
                    {
                        new Exercise()
                        {
                            ExerciseId = Guid.NewGuid(),
                            AddedByUserId = fakeUserId,
                            AddedDate = DateOnly.FromDateTime(DateTime.Now),
                            Name = "Bicep curls",
                            ExerciseSets = new List<ExerciseSet>()
                            {
                                new ExerciseSet()
                                {
                                    ExerciseSetId = Guid.NewGuid(),
                                    SetNumber = 1,
                                    Duration = 30,
                                    DurationUnit = Enums.DurationUnit.Minutes
                                }
                            }
                        }
                    }
                }
            };
            return Ok(mockWorkouts);
        }
    }
}
