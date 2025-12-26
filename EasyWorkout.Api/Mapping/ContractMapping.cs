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
                CompletedExerciseSets = [.. request.CompletedExerciseSets.Select(e => e.MapToCompletedExerciseSet(completedWorkoutId))]
            };
        }

        public static CompletedExerciseSet MapToCompletedExerciseSet(this FinishExerciseSetRequest request, Guid completedWorkoutId)
        {
            return new CompletedExerciseSet()
            {
                Id = Guid.NewGuid(),
                ExerciseSetId = request.ExerciseSetId,
                CompletedWorkoutId = completedWorkoutId,
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
            var completedWorkoutId = completedWorkout.Id;
            return new CompletedWorkoutResponse()
            {
                Id = completedWorkoutId,
                CompletedByUserId = completedWorkout.CompletedByUserId,
                WorkoutId = completedWorkout.WorkoutId,
                CompletedDate = completedWorkout.CompletedDate,
                CompletedNotes = completedWorkout.CompletedNotes,
                CompletedExerciseSets = completedWorkout.CompletedExerciseSets.Select(cs => cs.MapToResponse(completedWorkoutId))
            };
        }

        public static CompletedExerciseSetResponse MapToResponse(this CompletedExerciseSet completedSet, Guid completedWorkoutId)
        {
            return new CompletedExerciseSetResponse()
            {
                Id = completedSet.Id,
                ExerciseSetId = completedSet.ExerciseSetId,
                CompletedWorkoutId = completedWorkoutId,
                CompletedDate = completedSet.CompletedDate,
                SetNumber = completedSet.SetNumber,
                Reps = completedSet.Reps,
                Weight = completedSet.Weight,
                Duration = completedSet.Duration
            };
        }

        public static UserResponse MapToResponse(this User user)
        {
            return new UserResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JoinedDate = user.JoinedDate
            };
        }
    }
}
