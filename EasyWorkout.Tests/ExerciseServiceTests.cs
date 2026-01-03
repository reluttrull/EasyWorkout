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
    public class ExerciseServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_ExistingExercise_ShouldReturnExercise()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var exercise = new Exercise()
            {
                Id = Guid.NewGuid(),
                AddedByUserId = Guid.NewGuid(),
                AddedDate = DateOnly.FromDateTime(DateTime.Now),
                Name = "Exercise",
                LastEditedDate = DateTime.UtcNow
            };

            await using (var context = new WorkoutsContext(options))
            {
                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();
                var exerciseService = new ExerciseService(context);

                var result = await exerciseService.GetByIdAsync(exercise.Id);

                Assert.NotNull(result);
                Assert.Equal("Exercise", result.Name);
            }
        }

        [Fact]
        public async Task CreateAsync_ValidExercise_ShouldBeAddedToContext()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var userId = Guid.NewGuid();
                var exerciseId = Guid.NewGuid();
                var exercise = new Exercise()
                {
                    Id = exerciseId,
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Exercise",
                    LastEditedDate = DateTime.UtcNow
                };

                var success = await exerciseService.CreateAsync(exercise);
                Assert.True(success);
                Assert.NotEmpty(context.Exercises);

                //todo: separate
                var belongsToUser = await exerciseService.BelongsToUserAsync(exerciseId, userId);
                Assert.True(belongsToUser);
            }
        }

        [Fact]
        public async Task CreateAsync_DuplicateExercise_ShouldNotBeCreated()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var exerciseId = Guid.NewGuid();
                var exercise = new Exercise()
                {
                    Id = exerciseId,
                    AddedByUserId = Guid.NewGuid(),
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Workout",
                    LastEditedDate = DateTime.UtcNow
                };

                var success = await exerciseService.CreateAsync(exercise);
                success = await exerciseService.CreateAsync(exercise);
                Assert.False(success);
            }
        }

        [Fact]
        public async Task DeleteAsync_ExistingExercise_ShouldBeDeleted()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var exerciseId = Guid.NewGuid();
                var exercise = new Exercise()
                {
                    Id = exerciseId,
                    AddedByUserId = Guid.NewGuid(),
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Workout",
                    LastEditedDate = DateTime.UtcNow
                };

                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();

                var success = await exerciseService.DeleteAsync(exerciseId);
                Assert.True(success);
            }
        }

        [Fact]
        public async Task DeleteAsync_NonExistingExercise_ShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var success = await exerciseService.DeleteAsync(Guid.NewGuid());
                Assert.False(success);
            }
        }

        [Fact]
        public async Task GetAllForUserAsync_ShouldReturnOnlyCompletedWorkoutsOfUser()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var userId = Guid.NewGuid();
                var exercise = new Exercise()
                {
                    Id = Guid.NewGuid(),
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Exercise",
                    LastEditedDate = DateTime.UtcNow
                };
                context.Exercises.Add(exercise);
                exercise = new Exercise()
                {
                    Id = Guid.NewGuid(),
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Exercise 2",
                    LastEditedDate = DateTime.UtcNow
                };
                context.Exercises.Add(exercise);
                var otherExerciseId = Guid.NewGuid();
                exercise = new Exercise()
                {
                    Id = otherExerciseId,
                    AddedByUserId = Guid.NewGuid(),
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Somebody Else's Exercise",
                    LastEditedDate = DateTime.UtcNow
                };
                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();
                var exercises = await exerciseService.GetAllForUserAsync(userId);
                Assert.NotNull(exercises);
                Assert.Equal(exercises?.Count(), 2);

                // todo: separate
                var belongsToUser = await exerciseService.BelongsToUserAsync(otherExerciseId, userId);
                Assert.False(belongsToUser);
            }
        }

        [Fact]
        public async Task DeleteSetAsync_ExistingSet_ShouldBeDeletedAndRemovedFromExercise()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var userId = Guid.NewGuid();
                var exerciseId = Guid.NewGuid();
                var exercise = new Exercise()
                {
                    Id = exerciseId,
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Exercise",
                    LastEditedDate = DateTime.UtcNow
                };
                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();
                var setId = Guid.NewGuid();
                var set = new ExerciseSet()
                {
                    Id = setId,
                    ExerciseId = exerciseId,
                    SetNumber = 0
                };
                var success = await exerciseService.CreateSetAsync(exerciseId, set);
                success = await exerciseService.DeleteSetAsync(exerciseId, setId);
                Assert.True(success);
                var reloadedExercise = await exerciseService.GetByIdAsync(exerciseId);
                Assert.NotNull(reloadedExercise);
                Assert.Empty(reloadedExercise.ExerciseSets);
            }
        }

        [Fact]
        public async Task UpdateAsync_ValidRequest_ShouldUpdateExerciseProperties()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var exerciseId = Guid.NewGuid();
                var exercise = new Exercise()
                {
                    Id = exerciseId,
                    AddedByUserId = Guid.NewGuid(),
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Exercise",
                    LastEditedDate = DateTime.UtcNow
                };

                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();

                UpdateExerciseRequest request = new()
                {
                    Name = "New name",
                    Notes = "asdf"
                };
                var updatedExercise = await exerciseService.UpdateAsync(exerciseId, request);
                Assert.NotNull(updatedExercise);
                Assert.Equal("New name", updatedExercise.Name);
                Assert.Equal("asdf", updatedExercise.Notes);
            }
        }

        [Fact]
        public async Task GetNextSetIndexAsync_WhenNoSets_ShouldReturnZero()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var userId = Guid.NewGuid();
                var exerciseId = Guid.NewGuid();
                var exercise = new Exercise()
                {
                    Id = exerciseId,
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Exercise",
                    LastEditedDate = DateTime.UtcNow
                };

                var success = await exerciseService.CreateAsync(exercise);
                Assert.True(success);
                var nextSetIndex = await exerciseService.GetNextSetIndexAsync(exerciseId);
                Assert.Equal(0, nextSetIndex);
            }
        }
        [Fact]
        public async Task GetNextSetIndexAsync_WhenOneSet_ShouldReturnOne()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var exerciseService = new ExerciseService(context);
                var userId = Guid.NewGuid();
                var exerciseId = Guid.NewGuid();
                var exercise = new Exercise()
                {
                    Id = exerciseId,
                    AddedByUserId = userId,
                    AddedDate = DateOnly.FromDateTime(DateTime.Now),
                    Name = "Exercise",
                    LastEditedDate = DateTime.UtcNow
                };
                context.Exercises.Add(exercise);
                await context.SaveChangesAsync();

                var set = new ExerciseSet()
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = exerciseId,
                    SetNumber = 0
                };
                var success = await exerciseService.CreateSetAsync(exerciseId, set);
                var nextSetIndex = await exerciseService.GetNextSetIndexAsync(exerciseId);
                Assert.Equal(1, nextSetIndex);
            }
        }
    }
}
