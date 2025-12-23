using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly WorkoutsContext _workoutsContext;

        public WorkoutService(WorkoutsContext workoutsContext)
        {
            _workoutsContext = workoutsContext;
        }

        public async Task<bool> CreateAsync(Workout workout, CancellationToken token = default)
        {
            if (_workoutsContext.Workouts.Any(w => w.Name == workout.Name && w.AddedByUserId == workout.AddedByUserId))
                return false;

            _workoutsContext.Workouts.Add(workout);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var workoutToDelete = _workoutsContext.Workouts.Single(w => w.Id == id);

            _workoutsContext.Workouts.Remove(workoutToDelete);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<IEnumerable<Workout>> GetAllForUserAsync(Guid userId, CancellationToken token = default)
        {
            return _workoutsContext.Workouts
                .Where(w => w.AddedByUserId == userId)
                .Include(w => w.Exercises)
                    .ThenInclude(e => e.ExerciseSets)
                .AsEnumerable<Workout>();
        }

        public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var workout = _workoutsContext.Workouts.First(w => w.Id == id);
            if (workout is null) return null;

            await _workoutsContext.Entry(workout)
                .Collection(w => w.Exercises)
                .Query()
                .Include(e => e.ExerciseSets)
                .LoadAsync(token);

            return workout;
        }

        public async Task<bool> AddExerciseAsync(Guid id, Guid exerciseId, CancellationToken token = default)
        {
            var workout = await GetByIdAsync(id, token);
            if (workout is null) return false;
            if (workout.Exercises.Any(e => e.Id == exerciseId)) return false;

            var exercise = _workoutsContext.Exercises.Single(e => e.Id == exerciseId);
            if (exercise is null) return false;
            await _workoutsContext.Entry(exercise)
                .Collection(e => e.Workouts)
                .LoadAsync(token);

            workout.Exercises.Add(exercise);
            exercise.Workouts.Add(workout);

            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> RemoveExerciseAsync(Guid id, Guid exerciseId, CancellationToken token = default)
        {
            var workout = await GetByIdAsync(id, token);
            if (workout is null) return false;

            var exercise = _workoutsContext.Exercises.Single(e => e.Id == exerciseId);
            if (exercise is null) return false;
            await _workoutsContext.Entry(exercise)
                .Collection(e => e.Workouts)
                .LoadAsync(token);

            workout.Exercises.Remove(exercise);
            exercise.Workouts.Remove(workout);

            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<Workout?> UpdateAsync(Guid id, UpdateWorkoutRequest request, CancellationToken token = default)
        {
            var workoutToChange = _workoutsContext.Workouts.First(w => w.Id == id);
            if (workoutToChange is null) return null;

            workoutToChange.Name = request.Name;
            workoutToChange.Notes = request.Notes;

            await _workoutsContext.SaveChangesAsync(token);

            return workoutToChange;
        }

        public async Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default)
        {
            return _workoutsContext.Workouts.Any(w => w.Id == id && w.AddedByUserId == userId);
        }
    }
}
