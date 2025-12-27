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
        public async Task TestGetByIdAsyncSuccess()
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

                var result = await workoutService.GetByIdAsync(workout.Id);

                Assert.NotNull(result);
                Assert.Equal("Workout", result.Name);
            }
        }

        [Fact]
        public async Task TestGetByIdAsyncFailure()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var workoutService = new WorkoutService(context);

                var result = await workoutService.GetByIdAsync(Guid.NewGuid());

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task TestCreate()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var workoutService = new WorkoutService(context);
                var workoutId = Guid.NewGuid();
                var workout = new Workout()
                {
                    Id = workoutId,
                    AddedByUserId = Guid.NewGuid(),
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Workout",
                    Exercises = []
                };

                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);

                var result = await workoutService.GetByIdAsync(workoutId);

                Assert.NotNull(result);
                Assert.Equal("Workout", result.Name);
            }
        }
    }
}
