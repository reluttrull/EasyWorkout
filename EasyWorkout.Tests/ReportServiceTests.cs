using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Tests
{
    [Collection("EF")]
    public class ReportServiceTests
    {
        [Fact]
        public async Task GetTotalVolumeForUserAsync_WhenNoneExist_ShouldReturnEmpty()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var reportService = new ReportService(context);
                var results = await reportService.GetTotalVolumeForUserAsync(Guid.NewGuid(), null, null, null);
                Assert.Empty(results);
            }
        }

        [Fact]
        public async Task GetTotalVolumeForUserAsync_WhenOneExistsAndNoFilters_ShouldReturnOne()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var reportService = new ReportService(context);
                var userId = Guid.NewGuid();
                var completedWorkout = new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = [
                        new CompletedExercise()
                        {
                            Id = Guid.NewGuid(),
                            CompletedByUserId = userId,
                            CompletedDate = DateTime.UtcNow,
                            ExerciseNumber = 0,
                            CompletedExerciseSets = [
                                new CompletedExerciseSet()
                                {
                                    Id = Guid.NewGuid(),
                                    ExerciseSetId = Guid.NewGuid(),
                                    CompletedDate = DateTime.UtcNow,
                                    SetNumber = 0
                                }
                            ]
                        }
                    ]
                };

                context.CompletedWorkouts.Add(completedWorkout);
                await context.SaveChangesAsync();

                var results = await reportService.GetTotalVolumeForUserAsync(userId, null, null, null);
                Assert.Single(results);
            }
        }

        [Fact]
        public async Task GetTotalVolumeForUserAsync_WhenOneExistsAndOutOfRange_ShouldReturnNone()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var reportService = new ReportService(context);
                var userId = Guid.NewGuid();
                var completedWorkout = new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = [
                        new CompletedExercise()
                        {
                            Id = Guid.NewGuid(),
                            CompletedByUserId = userId,
                            CompletedDate = DateTime.UtcNow,
                            ExerciseNumber = 0,
                            CompletedExerciseSets = [
                                new CompletedExerciseSet()
                                {
                                    Id = Guid.NewGuid(),
                                    ExerciseSetId = Guid.NewGuid(),
                                    CompletedDate = DateTime.UtcNow,
                                    SetNumber = 0
                                }
                            ]
                        }
                    ]
                };

                context.CompletedWorkouts.Add(completedWorkout);
                await context.SaveChangesAsync();

                var results = await reportService.GetTotalVolumeForUserAsync(userId, null, DateTime.UtcNow.AddDays(-1), null);
                Assert.Empty(results);
            }
        }

        [Fact]
        public async Task GetTotalVolumeForUserAsync_WhenOneExistsAndDifferentWorkout_ShouldReturnNone()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var reportService = new ReportService(context);
                var userId = Guid.NewGuid();
                var completedWorkout = new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = [
                        new CompletedExercise()
                        {
                            Id = Guid.NewGuid(),
                            CompletedByUserId = userId,
                            CompletedDate = DateTime.UtcNow,
                            ExerciseNumber = 0,
                            CompletedExerciseSets = [
                                new CompletedExerciseSet()
                                {
                                    Id = Guid.NewGuid(),
                                    ExerciseSetId = Guid.NewGuid(),
                                    CompletedDate = DateTime.UtcNow,
                                    SetNumber = 0
                                }
                            ]
                        }
                    ]
                };

                context.CompletedWorkouts.Add(completedWorkout);
                await context.SaveChangesAsync();

                var results = await reportService.GetTotalVolumeForUserAsync(userId, null, null, Guid.NewGuid());
                Assert.Empty(results);
            }
        }

        [Fact]
        public async Task GetTotalVolumeForUserAsync_WhenOneExistsAndInRange_ShouldReturnOne()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var reportService = new ReportService(context);
                var userId = Guid.NewGuid();
                var completedWorkout = new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = [
                        new CompletedExercise()
                        {
                            Id = Guid.NewGuid(),
                            CompletedByUserId = userId,
                            CompletedDate = DateTime.UtcNow,
                            ExerciseNumber = 0,
                            CompletedExerciseSets = [
                                new CompletedExerciseSet()
                                {
                                    Id = Guid.NewGuid(),
                                    ExerciseSetId = Guid.NewGuid(),
                                    CompletedDate = DateTime.UtcNow,
                                    SetNumber = 0
                                }
                            ]
                        }
                    ]
                };

                context.CompletedWorkouts.Add(completedWorkout);
                await context.SaveChangesAsync();

                var results = await reportService.GetTotalVolumeForUserAsync(userId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow, null);
                Assert.Single(results);
            }
        }

        [Fact]
        public async Task GetTotalVolumeForUserAsync_WhenOneExistsAndMatchesWorkout_ShouldReturnOne()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new WorkoutsContext(options))
            {
                var reportService = new ReportService(context);
                var userId = Guid.NewGuid();
                var workoutId = Guid.NewGuid();
                var completedWorkout = new CompletedWorkout()
                {
                    Id = Guid.NewGuid(),
                    WorkoutId = workoutId,
                    CompletedByUserId = userId,
                    CompletedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow,
                    CompletedExercises = [
                        new CompletedExercise()
                        {
                            Id = Guid.NewGuid(),
                            CompletedByUserId = userId,
                            CompletedDate = DateTime.UtcNow,
                            ExerciseNumber = 0,
                            CompletedExerciseSets = [
                                new CompletedExerciseSet()
                                {
                                    Id = Guid.NewGuid(),
                                    ExerciseSetId = Guid.NewGuid(),
                                    CompletedDate = DateTime.UtcNow,
                                    SetNumber = 0
                                }
                            ]
                        }
                    ]
                };

                context.CompletedWorkouts.Add(completedWorkout);
                await context.SaveChangesAsync();

                var results = await reportService.GetTotalVolumeForUserAsync(userId, null, null, workoutId);
                Assert.Single(results);
            }
        }
    }
}
