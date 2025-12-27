using Castle.Components.DictionaryAdapter.Xml;
using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Application.Services;
using EasyWorkout.Identity.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Diagnostics;

namespace EasyWorkout.Tests
{
    public class WorkoutServiceTests
    {
        [Fact]
        public async Task Test1()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var workout = new Workout()
            {
                Id = Guid.NewGuid(),
                AddedByUserId = Guid.NewGuid(),
                AddedDate = DateOnly.FromDateTime(DateTime.Now),
                Name = "Workout",
                Exercises = []
            };

            await using (var context = new WorkoutsContext(options))
            {
                context.Workouts.Add(workout);
                await context.SaveChangesAsync();
                var workoutService = new WorkoutService(context);

                Debug.WriteLine(context.Workouts.Count());
                var result = await workoutService.GetByIdAsync(workout.Id);

                Assert.NotNull(result);
                Assert.Equal("Workout", result.Name);
            }
        }
    }
}
