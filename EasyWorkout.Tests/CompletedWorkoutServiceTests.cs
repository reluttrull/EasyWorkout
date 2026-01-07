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
        public async Task GetByIdAsync_ExistingCompletedWorkoutWithDependents_ShouldBeFetchedWithDependents()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
        public async Task CreateAsync_ValidCompletedWorkout_ShouldBeAddedToContext()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
                Assert.NotEmpty(context.CompletedWorkouts);

                // todo: separate
                var belongsToUser = await cwService.BelongsToUserAsync(cwId, userId);
                Assert.True(belongsToUser);
            }
        }

        [Fact]
        public async Task CreateAsync_DuplicateCompletedWorkout_ShouldNotBeCreated()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
                success = await cwService.CreateAsync(cw);
                Assert.False(success);
            }
        }

        [Fact]
        public async Task DeleteAsync_ExistingCompletedWorkout_ShouldBeDeleted()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
        public async Task DeleteAsync_NonExistingCompletedWorkout_ShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var cwService = new CompletedWorkoutService(context);
                var success = await cwService.DeleteAsync(Guid.NewGuid());
                Assert.False(success);
            }
        }

        [Fact]
        public async Task DeleteAllAsync_ShouldDeleteAllCompletedByUserId()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var cwService = new CompletedWorkoutService(context);
                var userId = Guid.NewGuid();

                List<CompletedWorkout> cws = [new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                },
                new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = Guid.NewGuid(),
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                },
                new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = []
                }];

                context.CompletedWorkouts.AddRange(cws);
                await context.SaveChangesAsync();

                var success = await cwService.DeleteAllAsync(userId);
                Assert.True(success);
                Assert.Equal(1, context.CompletedWorkouts.Count());
            }
        }


        [Fact]
        public async Task DeleteAllAsync_ShouldDeleteAllInChain()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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

                var ce = new CompletedExercise()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    ExerciseNumber = 0,
                    CompletedExerciseSets = []
                };

                var ces = new CompletedExerciseSet()
                {
                    Id = Guid.NewGuid(),
                    ExerciseSetId = null,
                    CompletedDate = DateTime.UtcNow,
                    SetNumber = 0
                };

                ce.CompletedExerciseSets.Add(ces);
                cw.CompletedExercises.Add(ce);

                context.CompletedWorkouts.Add(cw);
                await context.SaveChangesAsync();

                var success = await cwService.DeleteAllAsync(userId);
                Assert.True(success);
                Assert.Empty(context.CompletedWorkouts);
                Assert.Empty(context.CompletedExercises);
                Assert.Empty(context.CompletedExerciseSets);
            }
        }


        [Fact]
        public async Task GetAllForUserAsync_ShouldReturnOnlyCompletedWorkoutsOfUser()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
        public async Task UpdateAsync_ValidRequest_ShouldUpdateCompletedWorkoutProperties()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
