using Castle.Components.DictionaryAdapter.Xml;
using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Diagnostics;

namespace EasyWorkout.Tests
{
    [Collection("EF")]
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
                LastEditedDate = DateTime.UtcNow,
                WorkoutExercises = []
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
        public async Task TestCreate()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var workoutService = new WorkoutService(context);
                var userId = Guid.NewGuid();
                var workoutId = Guid.NewGuid();
                var workout = new Workout()
                {
                    Id = workoutId,
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Workout",
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };

                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);

                var result = await workoutService.GetByIdAsync(workoutId);

                Assert.NotNull(result);
                Assert.Equal("Workout", result.Name);

                var belongsToUser = await workoutService.BelongsToUserAsync(workoutId, userId);
                Assert.True(belongsToUser);
            }
        }

        [Fact]
        public async Task TestCreateDuplicate()
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
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };

                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);
                success = await workoutService.CreateAsync(workout);
                Assert.False(success);
            }
        }

        [Fact]
        public async Task TestDeleteSuccess()
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
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };

                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);
                success = await workoutService.DeleteAsync(workoutId);
                Assert.True(success);
            }
        }

        [Fact]
        public async Task TestDeleteNotFound()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var workoutService = new WorkoutService(context);
                var success = await workoutService.DeleteAsync(Guid.NewGuid());
                Assert.False(success);
            }
        }

        [Fact]
        public async Task TestGetAllForUser()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var workoutService = new WorkoutService(context);
                var userId = Guid.NewGuid();
                var workout = new Workout()
                {
                    Id = Guid.NewGuid(),
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Workout",
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };
                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);
                workout = new Workout()
                {
                    Id = Guid.NewGuid(),
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Workout 2",
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };
                success = await workoutService.CreateAsync(workout);
                Assert.True(success);
                var otherWorkoutId = Guid.NewGuid();
                workout = new Workout()
                {
                    Id = otherWorkoutId,
                    AddedByUserId = Guid.NewGuid(),
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Somebody Else's Workout",
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };
                success = await workoutService.CreateAsync(workout);
                Assert.True(success);
                var workouts = await workoutService.GetAllDetailedForUserAsync(userId);
                Assert.NotNull(workouts);
                Assert.Equal(workouts?.Count(), 2);

                var belongsToUser = await workoutService.BelongsToUserAsync(otherWorkoutId, userId);
                Assert.False(belongsToUser);
            }
        }

        [Fact]
        public async Task TestAddDeleteExercise()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var workoutService = new WorkoutService(context);
                var userId = Guid.NewGuid();
                var workoutId = Guid.NewGuid();
                var workout = new Workout()
                {
                    Id = workoutId,
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Workout",
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };

                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);
                var existingExerciseId = Guid.NewGuid();
                var existingExercise = new Exercise()
                {
                    Id = existingExerciseId,
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "",
                    LastEditedDate = DateTime.UtcNow
                };
                context.Exercises.Add(existingExercise);
                await context.SaveChangesAsync();

                success = await workoutService.AddExerciseAsync(workoutId, existingExerciseId);
                Assert.True(success);
                var reloadedWorkout = await workoutService.GetByIdAsync(workoutId);
                Assert.NotNull(reloadedWorkout);
                Assert.NotEmpty(reloadedWorkout.WorkoutExercises);
                var linkId = reloadedWorkout.WorkoutExercises[0]?.Id;
                Assert.NotNull(linkId);

                success = await workoutService.RemoveExerciseAsync(workoutId, existingExerciseId);
                Assert.True(success);
                reloadedWorkout = await workoutService.GetByIdAsync(workoutId);
                Assert.NotNull(reloadedWorkout);
                Assert.Empty(reloadedWorkout.WorkoutExercises);
            }
        }

        [Fact]
        public async Task TestUpdateSuccess()
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
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };

                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);

                UpdateWorkoutRequest request = new()
                {
                    Name = "New name",
                    Notes = "asdf"
                };
                var updatedWorkout = await workoutService.UpdateAsync(workoutId, request);
                Assert.NotNull(updatedWorkout);
                Assert.Equal("New name", updatedWorkout.Name);
                Assert.Equal("asdf", updatedWorkout.Notes);
            }
        }

        [Fact]
        public async Task TestGetLastCompletedDateNull()
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
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };

                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);

                var lastCompletedDate = await workoutService.GetLastCompletedDateAsync(workoutId);
                Assert.Null(lastCompletedDate);
            }
        }

        [Fact]
        public async Task TestGetLastCompletedDateNotNull()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var workoutService = new WorkoutService(context);
                var userId = Guid.NewGuid();
                var workoutId = Guid.NewGuid();
                var workout = new Workout()
                {
                    Id = workoutId,
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Workout",
                    LastEditedDate = DateTime.UtcNow,
                    WorkoutExercises = []
                };

                var success = await workoutService.CreateAsync(workout);
                Assert.True(success);

                CompletedWorkout completed = new()
                {
                    Id = Guid.NewGuid(),
                    WorkoutId = workoutId,
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };
                context.CompletedWorkouts.Add(completed);
                await context.SaveChangesAsync();

                var lastCompletedDate = await workoutService.GetLastCompletedDateAsync(workoutId);
                Assert.NotNull(lastCompletedDate);
            }
        }
    }
}
