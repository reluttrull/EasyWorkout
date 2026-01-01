using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Tests
{
    [Collection("EF")]
    public class CompletedWorkoutServiceTests
    {

        [Fact]
        public async Task TestGetByIdAsyncSuccess()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var completedWorkoutId = Guid.NewGuid();

                var completedWorkout = new CompletedWorkout()
                {
                    Id = completedWorkoutId,
                    CompletedByUserId = Guid.NewGuid(),
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };

                var completedExercise = new CompletedExercise()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = Guid.NewGuid(),
                    CompletedDate = DateTime.Now,
                    ExerciseNumber = 1
                };

                completedWorkout.CompletedExercises.Add(completedExercise);
                context.CompletedWorkouts.Add(completedWorkout);
                await context.SaveChangesAsync();

                var cwService = new CompletedWorkoutService(context);

                var result = await cwService.GetByIdAsync(completedWorkout.Id);

                Assert.NotNull(result);
                Assert.NotEmpty(result.CompletedExercises);
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
                var cwService = new CompletedWorkoutService(context);
                var userId = Guid.NewGuid();
                var cwId = Guid.NewGuid();
                var cw = new CompletedWorkout()
                {
                    Id = cwId,
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };

                var success = await cwService.CreateAsync(cw);
                Assert.True(success);

                var result = await cwService.GetByIdAsync(cwId);

                Assert.NotNull(result);
                Assert.Equal(userId, result.CompletedByUserId);

                var belongsToUser = await cwService.BelongsToUserAsync(cwId, userId);
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
                var cwService = new CompletedWorkoutService(context);
                var cwId = Guid.NewGuid();
                var cw = new CompletedWorkout()
                {
                    Id = cwId,
                    CompletedByUserId = Guid.NewGuid(),
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };

                var success = await cwService.CreateAsync(cw);
                Assert.True(success);
                success = await cwService.CreateAsync(cw);
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
                var cwService = new CompletedWorkoutService(context);
                var cwId = Guid.NewGuid();
                var cw = new CompletedWorkout()
                {
                    Id = cwId,
                    CompletedByUserId = Guid.NewGuid(),
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };

                var success = await cwService.CreateAsync(cw);
                Assert.True(success);
                success = await cwService.DeleteAsync(cwId);
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
                var cwService = new CompletedWorkoutService(context);
                var success = await cwService.DeleteAsync(Guid.NewGuid());
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
                var cwService = new CompletedWorkoutService(context);
                var userId = Guid.NewGuid();
                var cw = new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };
                var success = await cwService.CreateAsync(cw);
                Assert.True(success);
                cw = new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };
                success = await cwService.CreateAsync(cw);
                Assert.True(success);
                var otherCwId = Guid.NewGuid();
                cw = new CompletedWorkout()
                {
                    Id = otherCwId,
                    CompletedByUserId = Guid.NewGuid(),
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };
                success = await cwService.CreateAsync(cw);
                Assert.True(success);
                var completedWorkouts = await cwService.GetAllForUserAsync(userId);
                Assert.NotNull(completedWorkouts);
                Assert.Equal(completedWorkouts?.Count(), 2);

                var belongsToUser = await cwService.BelongsToUserAsync(otherCwId, userId);
                Assert.False(belongsToUser);
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
                var cwService = new CompletedWorkoutService(context);
                var cwId = Guid.NewGuid();
                var cw = new CompletedWorkout()
                {
                    Id = cwId,
                    CompletedByUserId = Guid.NewGuid(),
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                };

                var success = await cwService.CreateAsync(cw);
                Assert.True(success);

                UpdateCompletedWorkoutRequest request = new()
                {
                    CompletedNotes = "asdf"
                };
                var updatedCompletedWorkout = await cwService.UpdateAsync(cwId, request);
                Assert.NotNull(updatedCompletedWorkout);
                Assert.Equal("asdf", updatedCompletedWorkout.CompletedNotes);
            }
        }
    }
}
