using EasyWorkout.Application.Extensions;
using EasyWorkout.Application.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Tests
{
    public class CompletedWorkoutExtensionsTests
    {
        [Fact]
        public async Task GetTotalVolume_WhenMixedUnits_ShouldReturnInTargetUnits()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Reps = 2,
                                Weight = 3,
                                WeightUnit = Contracts.Model.Enums.WeightUnit.Kilograms
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Reps = 1,
                                Weight = 5,
                                WeightUnit = Contracts.Model.Enums.WeightUnit.Pounds
                            }]
                    }]
            };
            var result = cw.GetTotalVolume(Contracts.Model.Enums.WeightUnit.Pounds);
            Assert.Equal(18.2277m, result);
        }


        [Fact]
        public async Task GetTotalVolume_WhenExistSetsWithNoWeight_ShouldNotBeAffected()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Reps = 2,
                                Weight = 3,
                                WeightUnit = Contracts.Model.Enums.WeightUnit.Kilograms
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Reps = 1,
                                Distance = 5,
                                DistanceUnit = Contracts.Model.Enums.DistanceUnit.Miles
                            }]
                    }]
            };
            var result = cw.GetTotalVolume(Contracts.Model.Enums.WeightUnit.Grams);
            Assert.Equal(6000m, result);
        }

        [Fact]
        public async Task GetTotalVolume_WhenNoSetsWithWeight_ShouldReturnZero()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Reps = 2,
                                Duration = 5,
                                DurationUnit = Contracts.Model.Enums.DurationUnit.Minutes
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Reps = 3
                            }]
                    }]
            };
            var result = cw.GetTotalVolume(Contracts.Model.Enums.WeightUnit.Pounds);
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetTotalTime_WhenMixedUnits_ShouldReturnInTargetUnits()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Duration = 5,
                                DurationUnit = Contracts.Model.Enums.DurationUnit.Minutes
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Duration = 1,
                                DurationUnit = Contracts.Model.Enums.DurationUnit.Hours
                            }]
                    }]
            };
            var result = cw.GetTotalTime(Contracts.Model.Enums.DurationUnit.Minutes);
            Assert.Equal(65, result);
        }

        [Fact]
        public async Task GetTotalTime_WhenExistSetsWithNoTime_ShouldNotBeAffected()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Reps = 2,
                                Duration = 5,
                                DurationUnit = Contracts.Model.Enums.DurationUnit.Minutes
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Reps = 3
                            }]
                    }]
            };
            var result = cw.GetTotalTime(Contracts.Model.Enums.DurationUnit.Hours);
            Assert.Equal(0.0833m, result);
        }

        [Fact]
        public async Task GetTotalTime_WhenNoSetsWithTime_ShouldReturnZero()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Reps = 2,
                                Weight = 5,
                                WeightUnit = Contracts.Model.Enums.WeightUnit.Pounds
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Reps = 3
                            }]
                    }]
            };
            var result = cw.GetTotalTime(Contracts.Model.Enums.DurationUnit.Seconds);
            Assert.Equal(0, result);
        }
        //

        [Fact]
        public async Task GetTotalDistance_WhenMixedUnits_ShouldReturnInTargetUnits()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Distance = 2,
                                DistanceUnit = Contracts.Model.Enums.DistanceUnit.Kilometers
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Distance = 500,
                                DistanceUnit = Contracts.Model.Enums.DistanceUnit.Meters
                            }]
                    }]
            };
            var result = cw.GetTotalDistance(Contracts.Model.Enums.DistanceUnit.Meters);
            Assert.Equal(2500m, result);
        }

        [Fact]
        public async Task GetTotalDistance_WhenExistSetsWithNoDistance_ShouldNotBeAffected()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Reps = 2,
                                Duration = 5,
                                DurationUnit = Contracts.Model.Enums.DurationUnit.Minutes
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Distance = 5,
                                DistanceUnit = Contracts.Model.Enums.DistanceUnit.Miles
                            }]
                    }]
            };
            var result = cw.GetTotalDistance(Contracts.Model.Enums.DistanceUnit.Miles);
            Assert.Equal(5m, result);
        }

        [Fact]
        public async Task GetTotalDistance_WhenNoSetsWithDistance_ShouldReturnZero()
        {
            CompletedWorkout cw = new CompletedWorkout()
            {
                Id = Guid.NewGuid(),
                CompletedByUserId = Guid.NewGuid(),
                CompletedDate = DateTime.UtcNow,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [
                    new CompletedExercise()
                    {
                        Id = Guid.NewGuid(),
                        CompletedByUserId = Guid.NewGuid(),
                        CompletedDate = DateTime.UtcNow,
                        ExerciseNumber = 0,
                        CompletedExerciseSets = [
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 0,
                                Reps = 2,
                                Weight = 5,
                                WeightUnit = Contracts.Model.Enums.WeightUnit.Pounds
                            },
                            new CompletedExerciseSet()
                            {
                                Id = Guid.NewGuid(),
                                ExerciseSetId = null,
                                CompletedDate = DateTime.UtcNow,
                                SetNumber = 1,
                                Reps = 3
                            }]
                    }]
            };
            var result = cw.GetTotalDistance(Contracts.Model.Enums.DistanceUnit.Laps);
            Assert.Equal(0, result);
        }
    }
}
