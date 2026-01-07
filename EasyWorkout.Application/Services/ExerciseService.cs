using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly WorkoutsContext _workoutsContext;

        public ExerciseService(WorkoutsContext workoutsContext)
        {
            _workoutsContext = workoutsContext;
        }

        public async Task<bool> CreateAsync(Exercise exercise, CancellationToken token = default)
        {
            if (_workoutsContext.Exercises.Any(e => e.Name == exercise.Name && e.AddedByUserId == exercise.AddedByUserId))
                return false;

            _workoutsContext.Exercises.Add(exercise);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var exerciseToDelete = _workoutsContext.Exercises.SingleOrDefault(e => e.Id == id);

            if (exerciseToDelete is null) return false;

            _workoutsContext.Exercises.Remove(exerciseToDelete);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> DeleteAllAsync(Guid userId, CancellationToken token)
        {
            var exercisesToDelete = _workoutsContext.Exercises
                .Where(w => w.AddedByUserId == userId);

            _workoutsContext.ExerciseSets.RemoveRange(exercisesToDelete.SelectMany(e => e.ExerciseSets));
            _workoutsContext.Exercises.RemoveRange(exercisesToDelete);

            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> CreateSetAsync(Guid id, ExerciseSet set, CancellationToken token = default)
        {
            var exercise = await GetByIdAsync(id);
            if (exercise is null) return false;

            if (_workoutsContext.ExerciseSets.Any(es => es.Id == set.Id)) return false;

            _workoutsContext.ExerciseSets.Add(set);
            exercise.ExerciseSets.Add(set);
            exercise.LastEditedDate = DateTime.UtcNow;
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<bool> DeleteSetAsync(Guid id, Guid exerciseSetId, CancellationToken token = default)
        {
            var exercise = await GetByIdAsync(id);
            if (exercise is null) return false;

            var set = _workoutsContext.ExerciseSets.FirstOrDefault(es => es.Id == exerciseSetId);
            if (set is null) return false;

            _workoutsContext.ExerciseSets.Remove(set);
            exercise.ExerciseSets.Remove(set);
            exercise.LastEditedDate = DateTime.UtcNow;
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<IEnumerable<Exercise>> GetAllForUserAsync(Guid userId, CancellationToken token = default)
        {
            return await _workoutsContext.Exercises
                .Where(e => e.AddedByUserId == userId)
                .Include(e => e.ExerciseSets)
                .ToListAsync();
        }

        public async Task<Exercise?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var exercise = await _workoutsContext.Exercises
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise is null) return null;

            await _workoutsContext.Entry(exercise)
                .Collection(e => e.ExerciseSets)
                .LoadAsync(token);

            return exercise;
        }

        public async Task<ExerciseSet?> GetSetByIdAsync(Guid id, CancellationToken token = default)
        {
            var exerciseSet = await _workoutsContext.ExerciseSets
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exerciseSet is null) return null;

            return exerciseSet;
        }


        public async Task<Exercise?> UpdateAsync(Guid id, UpdateExerciseRequest request, CancellationToken token = default)
        {
            var exerciseToChange = await _workoutsContext.Exercises.FirstOrDefaultAsync(e => e.Id == id);
            if (exerciseToChange is null) return null;

            exerciseToChange.Name = request.Name;
            exerciseToChange.Notes = request.Notes;
            exerciseToChange.LastEditedDate = DateTime.UtcNow;

            await _workoutsContext.SaveChangesAsync(token);

            return exerciseToChange;
        }
        public async Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default)
        {
            return await _workoutsContext.Exercises.AnyAsync(e => e.Id == id && e.AddedByUserId == userId);
        }

        public async Task<int?> GetNextSetIndexAsync(Guid id, CancellationToken token = default)
        {
            var exercise = await GetByIdAsync(id, token);
            if (exercise is null) return null;

            var sortedSetNumbers = exercise.ExerciseSets.Select(es => es.SetNumber).OrderByDescending(sn => sn);
            if (sortedSetNumbers.Count() == 0) return 0;

            return sortedSetNumbers.FirstOrDefault() + 1;
        }
    }
}
