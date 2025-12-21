using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Contracts.Responses;
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
                DurationUnit = exerciseSet.DurationUnit.ToString(),
                Notes = exerciseSet.Notes
            };
        }
    }
}
