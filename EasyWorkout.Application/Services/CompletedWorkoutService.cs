using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public class CompletedWorkoutService : ICompletedWorkoutService
    {
        private readonly WorkoutsContext _workoutsContext;

        public CompletedWorkoutService(WorkoutsContext workoutsContext)
        {
            _workoutsContext = workoutsContext;
        }

        public async Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default)
        {
            return await _workoutsContext.CompletedWorkouts.AnyAsync(cw => cw.Id == id && cw.CompletedByUserId == userId);
        }

        public async Task<bool> CreateAsync(CompletedWorkout workout, CancellationToken token = default)
        {
            if (_workoutsContext.CompletedWorkouts.Any(cw => cw.Id == workout.Id))
                return false;

            _workoutsContext.CompletedWorkouts.Add(workout);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var completedWorkoutToDelete = await _workoutsContext.CompletedWorkouts.SingleAsync(cw => cw.Id == id);

            _workoutsContext.CompletedWorkouts.Remove(completedWorkoutToDelete);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<IEnumerable<CompletedWorkoutDetailed?>> GetAllDetailedForUserAsync(Guid userId, CancellationToken token = default)
        {
            return await _workoutsContext.CompletedWorkouts
                .Where(cw => cw.CompletedByUserId == userId)
                .Include(cw => cw.CompletedExerciseSets)
                .Select(cw => new CompletedWorkoutDetailed(
                    cw, 
                    _workoutsContext.Workouts.Single(w => w.Id == cw.WorkoutId),
                    cw.CompletedExerciseSets
                        .Select(cs => new CompletedExerciseSetDetailed(
                            cs,
                            _workoutsContext.ExerciseSets
                                .Single(s => s.Id == cs.ExerciseSetId),
                            _workoutsContext.Exercises
                                .Single(e => e.ExerciseSets.Any(s => s.Id == cs.ExerciseSetId))
                                .Name))))
                .ToListAsync(token);
        }

        public async Task<IEnumerable<CompletedWorkout>> GetAllForUserAsync(Guid userId, CancellationToken token = default)
        {
            return await _workoutsContext.CompletedWorkouts
                .Where(cw => cw.CompletedByUserId == userId)
                .Include(cw => cw.CompletedExerciseSets)
                .ToListAsync();
        }

        public async Task<CompletedWorkout?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var completedWorkout = _workoutsContext.CompletedWorkouts.First(cw => cw.Id == id);
            if (completedWorkout is null) return null;

            await _workoutsContext.Entry(completedWorkout)
                .Collection(cw => cw.CompletedExerciseSets)
                .LoadAsync(token);

            return completedWorkout;
        }

        public async Task<CompletedWorkoutDetailed?> GetByIdDetailedAsync(Guid id, CancellationToken token = default)
        {
            var completedWorkout = _workoutsContext.CompletedWorkouts.First(cw => cw.Id == id);
            if (completedWorkout is null) return null;

            return await _workoutsContext.CompletedWorkouts
                .Where(cw => cw.Id == id)
                .Include(cw => cw.CompletedExerciseSets)
                .Select(cw => new CompletedWorkoutDetailed(
                    cw,
                    _workoutsContext.Workouts
                        .Single(w => w.Id == cw.WorkoutId),
                    cw.CompletedExerciseSets
                        .Select(cs => new CompletedExerciseSetDetailed(
                            cs,
                            _workoutsContext.ExerciseSets
                                .Single(s => s.Id == cs.ExerciseSetId),
                            _workoutsContext.Exercises
                                .Single(e => e.ExerciseSets.Any(s => s.Id == cs.ExerciseSetId))
                                .Name))))
                .SingleOrDefaultAsync();
        }

        public async Task<CompletedWorkoutDetailed?> UpdateAsync(Guid id, UpdateCompletedWorkoutRequest request, CancellationToken token = default)
        {
            var completedWorkoutToChange = await GetByIdDetailedAsync(id, token);
            if (completedWorkoutToChange is null) return null;

            completedWorkoutToChange.CompletedWorkout.CompletedNotes = request.CompletedNotes;

            await _workoutsContext.SaveChangesAsync(token);

            return completedWorkoutToChange;
        }
    }
}
