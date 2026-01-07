using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Contracts.Responses;
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
            if (await _workoutsContext.Workouts.AnyAsync(w => w.Name == workout.Name && w.AddedByUserId == workout.AddedByUserId))
                return false;

            _workoutsContext.Workouts.Add(workout);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var workoutToDelete = await _workoutsContext.Workouts.SingleOrDefaultAsync(w => w.Id == id);

            if (workoutToDelete is null) return false;

            _workoutsContext.Workouts.Remove(workoutToDelete);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> DeleteAllAsync(Guid userId, CancellationToken token)
        {
            var workoutsToDelete = _workoutsContext.Workouts
                .Where(w => w.AddedByUserId == userId);

            _workoutsContext.Workouts.RemoveRange(workoutsToDelete);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<IEnumerable<WorkoutDetailed>> GetAllDetailedForUserAsync(Guid userId, CancellationToken token = default)
        {
            return await _workoutsContext.Workouts
                .Where(w => w.AddedByUserId == userId)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                        .ThenInclude(e => e.ExerciseSets)
                .Select(w => new WorkoutDetailed(
                    w,
                    _workoutsContext.CompletedWorkouts
                        .Where(cw => cw.WorkoutId == w.Id)
                        .Max(cw => (DateTime?)cw.CompletedDate)
                ))
                .ToListAsync(token);
        }

        public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var workout = await _workoutsContext.Workouts.FirstOrDefaultAsync(w => w.Id == id);
            if (workout is null) return null;

            await _workoutsContext.Entry(workout)
                .Collection(w => w.WorkoutExercises)
                .Query()
                .Include(we => we.Exercise)
                    .ThenInclude(e => e.ExerciseSets)
                .LoadAsync(token);

            return workout;
        }

        public async Task<WorkoutDetailed?> GetByIdDetailedAsync(Guid id, CancellationToken token = default)
        {
            var workout = await _workoutsContext.Workouts.FirstOrDefaultAsync(w => w.Id == id);
            if (workout is null) return null;

            await _workoutsContext.Entry(workout)
                .Collection(w => w.WorkoutExercises)
                .Query()
                .Include(we => we.Exercise)
                    .ThenInclude(e => e.ExerciseSets)
                .LoadAsync(token);

            var lastCompletedDate = await GetLastCompletedDateAsync(workout.Id, token);

            return new WorkoutDetailed(workout, lastCompletedDate);
        }

        public async Task<bool> AddExerciseAsync(Guid workoutId, Guid exerciseId, CancellationToken token = default)
        {
            var workout = await _workoutsContext.Workouts
                .Include(w => w.WorkoutExercises)
                .SingleOrDefaultAsync(w => w.Id == workoutId, token);

            if (workout is null) return false;

            if (workout.WorkoutExercises.Any(we => we.ExerciseId == exerciseId))
                return false; // link already exists

            var exerciseToAdd = await _workoutsContext.Exercises.SingleOrDefaultAsync(e => e.Id == exerciseId, token);

            if (exerciseToAdd is null) return false;

            var nextExerciseNumber = workout.WorkoutExercises.Count == 0
                ? 0
                : workout.WorkoutExercises.Max(we => we.ExerciseNumber) + 1;

            var workoutExerciseLink = new WorkoutExercise
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout.Id,
                ExerciseId = exerciseToAdd.Id,
                ExerciseNumber = nextExerciseNumber
            };

            workout.WorkoutExercises.Add(workoutExerciseLink);
            workout.LastEditedDate = DateTime.UtcNow;

            _workoutsContext.WorkoutExercises.Add(workoutExerciseLink);

            var success = await _workoutsContext.SaveChangesAsync(token) > 0;

            return success;
        }

        public async Task<bool> RemoveExerciseAsync(Guid workoutId, Guid exerciseId, CancellationToken token = default)
        {
            var workoutExercise = await _workoutsContext.WorkoutExercises
                .FirstOrDefaultAsync(
                    we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId,
                    token);

            if (workoutExercise is null) return false;

            _workoutsContext.WorkoutExercises.Remove(workoutExercise);

            var workoutToUpdate = await GetByIdAsync(workoutId);
            workoutToUpdate?.LastEditedDate = DateTime.UtcNow;

            var success = await _workoutsContext.SaveChangesAsync(token) > 0;

            return success;
        }

        public async Task<Workout?> UpdateAsync(Guid id, UpdateWorkoutRequest request, CancellationToken token = default)
        {
            var workoutToChange = await _workoutsContext.Workouts.SingleOrDefaultAsync(w => w.Id == id);
            if (workoutToChange is null) return null;

            workoutToChange.Name = request.Name;
            workoutToChange.Notes = request.Notes;
            workoutToChange.LastEditedDate = DateTime.UtcNow;

            await _workoutsContext.SaveChangesAsync(token);

            return workoutToChange;
        }

        public async Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default)
        {
            return await _workoutsContext.Workouts.AnyAsync(w => w.Id == id && w.AddedByUserId == userId, token);
        }

        public async Task<DateTime?> GetLastCompletedDateAsync(Guid workoutId, CancellationToken token = default)
        {
            return await _workoutsContext.CompletedWorkouts
                .Where(cw => cw.WorkoutId == workoutId)
                .OrderByDescending(cw => cw.CompletedDate)
                .Select(cw => (DateTime?)cw.CompletedDate)
                .FirstOrDefaultAsync(token);
        }
    }
}
