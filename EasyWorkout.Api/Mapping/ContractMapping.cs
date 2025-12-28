using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Contracts.Responses;
using EasyWorkout.Identity.Api.Model;
using System.Runtime.CompilerServices;
using static EasyWorkout.Api.Endpoints;

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
                Exercises = []
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
                Exercises = workout.Exercises
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
                Workouts = [],
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
                Workouts = exercise.Workouts,
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
                DurationUnit = request.DurationUnit
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
                CompletedExercises = [.. request.CompletedExercises.Select(e => e.MapToCompletedExercise(userId))]
            };
        }

        public static CompletedExercise MapToCompletedExercise(this FinishExerciseRequest request, Guid userId)
        {
            return new CompletedExercise()
            {
                Id = Guid.NewGuid(),
                ExerciseId = request.ExerciseId,
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
                Duration = request.Duration
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
                Exercises = workout.Exercises.Select(e => e.MapToResponse())
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
                Exercises = workout.Exercises.Select(e => e.MapToResponse())
            };
        }

        public static ExerciseResponse MapToResponse(this Exercise exercise)
        {
            return new ExerciseResponse()
            {
                Id = exercise.Id,
                AddedByUserId = exercise.AddedByUserId,
                AddedDate = exercise.AddedDate,
                Name = exercise.Name,
                Notes = exercise.Notes,
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
                WeightUnit = exerciseSet.WeightUnit.ToString(),
                Duration = exerciseSet.Duration,
                DurationUnit = exerciseSet.DurationUnit.ToString()
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
                WeightUnit = completedSet.WeightUnit.ToString(),
                Duration = completedSet.Duration,
                GoalDuration = completedSet.Duration,
                DurationUnit = completedSet.DurationUnit.ToString()
            };
        }
    }
}
