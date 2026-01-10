using EasyWorkout.Application.Extensions;
using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Contracts.Responses;
using EasyWorkout.Identity.Api.Model;
using System.Runtime.CompilerServices;
using static EasyWorkout.Api.Endpoints;
using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Api.Mapping
{
    public static class ContractMapping
    {
        public static Workout MapToWorkout(this CreateWorkoutRequest request, Guid userId)
        {
            return new Workout()
            {
                Id = Guid.NewGuid(),
                AddedByUserId = userId,
                AddedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Name = request.Name,
                Notes = request.Notes,
                LastEditedDate = DateTime.UtcNow,
                WorkoutExercises = []
            };
        }

        public static Workout MapToWorkout(this UpdateWorkoutRequest request, Workout workout)
        {
            return new Workout()
            {
                Id = workout.Id,
                AddedByUserId = workout.AddedByUserId,
                AddedDate = workout.AddedDate,
                Name = request.Name,
                Notes = request.Notes,
                LastEditedDate = DateTime.UtcNow,
                WorkoutExercises = workout.WorkoutExercises
            };
        }

        public static Exercise MapToExercise(this CreateExerciseRequest request, Guid userId)
        {
            return new Exercise()
            {
                Id = Guid.NewGuid(),
                AddedByUserId = userId,
                AddedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Name = request.Name,
                Notes = request.Notes,
                LastEditedDate = DateTime.UtcNow,
                WorkoutExercises = [],
                ExerciseSets = []
            };
        }

        public static Exercise MapToExercise(this UpdateExerciseRequest request, Exercise exercise)
        {
            return new Exercise()
            {
                Id = exercise.Id,
                AddedByUserId = exercise.AddedByUserId,
                AddedDate = exercise.AddedDate,
                Name = request.Name,
                Notes = request.Notes,
                LastEditedDate = DateTime.UtcNow,
                WorkoutExercises = exercise.WorkoutExercises,
                ExerciseSets = exercise.ExerciseSets
            };
        }

        public static ExerciseSet MapToExerciseSet(this CreateSetRequest request, Guid exerciseId, int? setNumber)
        {
            return new ExerciseSet()
            {
                Id = Guid.NewGuid(),
                ExerciseId = exerciseId,
                SetNumber = setNumber ?? 0,
                Reps = request.Reps,
                Weight = request.Weight,
                WeightUnit = request.WeightUnit,
                Duration = request.Duration,
                DurationUnit = request.DurationUnit,
                Distance = request.Distance,
                DistanceUnit = request.DistanceUnit
            };
        }

        public static CompletedWorkout MapToCompletedWorkout(this FinishWorkoutRequest request, Guid userId)
        {
            Guid completedWorkoutId = Guid.NewGuid();
            return new CompletedWorkout()
            {
                Id = completedWorkoutId,
                CompletedByUserId = userId,
                WorkoutId = request.WorkoutId,
                CompletedDate = request.CompletedDate,
                CompletedNotes = request.CompletedNotes,
                LastEditedDate = DateTime.UtcNow,
                CompletedExercises = [.. request.CompletedExercises.Select(e => e.MapToCompletedExercise(userId))]
            };
        }

        public static CompletedExercise MapToCompletedExercise(this FinishExerciseRequest request, Guid userId)
        {
            return new CompletedExercise()
            {
                Id = Guid.NewGuid(),
                ExerciseId = request.ExerciseId,
                FallbackName = request.FallbackName,
                ExerciseNumber = request.ExerciseNumber,
                CompletedByUserId = userId,
                CompletedDate = request.CompletedDate,
                CompletedExerciseSets = [.. request.CompletedExerciseSets.Select(ces => ces.MapToCompletedExerciseSet())]
            };
        }

        public static CompletedExerciseSet MapToCompletedExerciseSet(this FinishExerciseSetRequest request)
        {
            return new CompletedExerciseSet()
            {
                Id = Guid.NewGuid(),
                ExerciseSetId = request.ExerciseSetId,
                CompletedDate = request.CompletedDate,
                SetNumber = request.SetNumber,
                Reps = request.Reps,
                Weight = request.Weight,
                Duration = request.Duration,
                Distance = request.Distance,
                GoalReps = request.GoalReps,
                GoalWeight = request.GoalWeight,
                GoalDuration = request.GoalDuration,
                GoalDistance = request.GoalDistance,
                WeightUnit = request.WeightUnit,
                DurationUnit = request.DurationUnit,
                DistanceUnit = request.DistanceUnit
            };
        }

        public static WorkoutResponse MapToResponse(this Workout workout, DateTime? lastCompletedDate)
        {
            return new WorkoutResponse()
            {
                Id = workout.Id,
                AddedByUserId = workout.AddedByUserId,
                AddedDate = workout.AddedDate,
                Name = workout.Name,
                Notes = workout.Notes,
                LastCompletedDate = lastCompletedDate,
                LastEditedDate = workout.LastEditedDate,
                Exercises = workout.WorkoutExercises
                    .OrderBy(we => we.ExerciseNumber)
                    .Select(we => we.Exercise.MapToResponse(we.ExerciseNumber))
            };
        }

        public static WorkoutResponse MapToResponse(this Workout workout)
        {
            return new WorkoutResponse()
            {
                Id = workout.Id,
                AddedByUserId = workout.AddedByUserId,
                AddedDate = workout.AddedDate,
                Name = workout.Name,
                Notes = workout.Notes,
                LastEditedDate = workout.LastEditedDate,
                Exercises = workout.WorkoutExercises
                    .OrderBy(we => we.ExerciseNumber)
                    .Select(we => we.Exercise.MapToResponse(we.ExerciseNumber))
            };
        }

        public static ExerciseResponse MapToResponse(this Exercise exercise, int exerciseNumber)
        {
            return new ExerciseResponse()
            {
                Id = exercise.Id,
                AddedByUserId = exercise.AddedByUserId,
                AddedDate = exercise.AddedDate,
                ExerciseNumber = exerciseNumber,
                Name = exercise.Name,
                Notes = exercise.Notes,
                LastEditedDate = exercise.LastEditedDate,
                ExerciseSets = exercise.ExerciseSets.Select(es => es.MapToResponse())
            };
        }

        public static ExerciseResponse MapToResponse(this Exercise exercise)
        {
            return new ExerciseResponse()
            {
                Id = exercise.Id,
                AddedByUserId = exercise.AddedByUserId,
                AddedDate = exercise.AddedDate,
                ExerciseNumber = -1, // when not attached to workout
                Name = exercise.Name,
                Notes = exercise.Notes,
                LastEditedDate = exercise.LastEditedDate,
                ExerciseSets = exercise.ExerciseSets.Select(es => es.MapToResponse())
            };
        }

        public static ExerciseSetResponse MapToResponse(this ExerciseSet exerciseSet)
        {
            return new ExerciseSetResponse()
            {
                Id = exerciseSet.Id,
                ExerciseId = exerciseSet.ExerciseId,
                SetNumber = exerciseSet.SetNumber,
                Reps = exerciseSet.Reps,
                Weight = exerciseSet.Weight,
                WeightUnit = exerciseSet.WeightUnit?.ToString(),
                Duration = exerciseSet.Duration,
                DurationUnit = exerciseSet.DurationUnit?.ToString(),
                Distance = exerciseSet.Distance,
                DistanceUnit = exerciseSet.DistanceUnit?.ToString()
            };
        }

        public static CompletedWorkoutResponse MapToResponse(this CompletedWorkout completedWorkout)
        {
            return new CompletedWorkoutResponse()
            {
                Id = completedWorkout.Id,
                CompletedByUserId = completedWorkout.CompletedByUserId,
                WorkoutId = completedWorkout.WorkoutId,
                CompletedDate = completedWorkout.CompletedDate,
                CompletedNotes = completedWorkout.CompletedNotes,
                OriginalName = completedWorkout.Name,
                OriginalNotes = completedWorkout.OriginalNotes,
                LastEditedDate = completedWorkout.LastEditedDate,
                CompletedExercises = completedWorkout.CompletedExercises.Select(ce => ce.MapToResponse())
            };
        }

        public static CompletedExerciseResponse MapToResponse(this CompletedExercise completedExercise)
        {
            return new CompletedExerciseResponse()
            {
                Id = completedExercise.Id,
                ExerciseId = completedExercise.ExerciseId,
                CompletedByUserId = completedExercise.CompletedByUserId,
                Name = completedExercise.Name,
                OriginalNotes = completedExercise.OriginalNotes,
                CompletedNotes = completedExercise.CompletedNotes,
                CompletedDate = completedExercise.CompletedDate,
                CompletedExerciseSets = completedExercise.CompletedExerciseSets.Select(ces => ces.MapToResponse())
            };
        }

        public static CompletedExerciseSetResponse MapToResponse(this CompletedExerciseSet completedSet)
        {
            return new CompletedExerciseSetResponse()
            {
                Id = completedSet.Id,
                ExerciseSetId = completedSet.ExerciseSetId,
                CompletedDate = completedSet.CompletedDate,
                SetNumber = completedSet.SetNumber,
                Reps = completedSet.Reps,
                GoalReps = completedSet.GoalReps,
                Weight = completedSet.Weight,
                GoalWeight = completedSet.GoalWeight,
                WeightUnit = completedSet.WeightUnit?.ToString(),
                Duration = completedSet.Duration,
                GoalDuration = completedSet.GoalDuration,
                DurationUnit = completedSet.DurationUnit?.ToString(),
                Distance = completedSet.Distance,
                GoalDistance = completedSet.GoalDistance,
                DistanceUnit = completedSet.DistanceUnit?.ToString()
            };
        }

        public static TotalVolumeReportResponse MapToResponse(this IEnumerable<CompletedWorkout> completedWorkouts, WeightUnit weightUnit)
        {
            return new TotalVolumeReportResponse()
            {
                WeightUnit = weightUnit,
                DataPoints = [.. completedWorkouts.Select(cw => new DataPointResponse(cw.Id, cw.CompletedDate, cw.GetTotalVolume(weightUnit)))]
            };
        }

        public static TotalTimeReportResponse MapToResponse(this IEnumerable<CompletedWorkout> completedWorkouts, DurationUnit durationUnit)
        {
            return new TotalTimeReportResponse()
            {
                DurationUnit = durationUnit,
                DataPoints = [.. completedWorkouts.Select(cw => new DataPointResponse(cw.Id, cw.CompletedDate, cw.GetTotalTime(durationUnit)))]
            };
        }

        public static TotalDistanceReportResponse MapToResponse(this IEnumerable<CompletedWorkout> completedWorkouts, DistanceUnit distanceUnit)
        {
            return new TotalDistanceReportResponse()
            {
                DistanceUnit = distanceUnit,
                DataPoints = [.. completedWorkouts.Select(cw => new DataPointResponse(cw.Id, cw.CompletedDate, cw.GetTotalDistance(distanceUnit)))]
            };
        }

        public static AveragePercentCompletedReportResponse MapToResponse(this IEnumerable<CompletedWorkout> completedWorkouts)
        {
            return new AveragePercentCompletedReportResponse()
            {
                DataPoints = [.. completedWorkouts.Select(cw => new DataPointResponse(cw.Id, cw.CompletedDate, cw.GetAveragePercentCompleted()))]
            };
        }
    }
}
