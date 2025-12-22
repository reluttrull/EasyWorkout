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
            var exerciseToDelete = _workoutsContext.Exercises.Single(e => e.Id == id);

            _workoutsContext.Exercises.Remove(exerciseToDelete);
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
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<IEnumerable<Exercise>> GetAllForUserAsync(Guid userId, CancellationToken token = default)
        {
            return _workoutsContext.Exercises
                .Where(e => e.AddedByUserId == userId)
                .Include(e => e.ExerciseSets)
                .AsEnumerable<Exercise>();
        }

        public async Task<Exercise?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var exercise = _workoutsContext.Exercises
                .FirstOrDefault(e => e.Id == id);

            if (exercise is null) return null;

            await _workoutsContext.Entry(exercise)
                .Collection(e => e.ExerciseSets)
                .LoadAsync(token);

            return exercise;
        }

        public async Task<Exercise?> UpdateAsync(Guid id, UpdateExerciseRequest request, CancellationToken token = default)
        {
            var exerciseToChange = _workoutsContext.Exercises.First(e => e.Id == id);
            if (exerciseToChange is null) return null;

            exerciseToChange.Name = request.Name;
            exerciseToChange.Notes = request.Notes;

            await _workoutsContext.SaveChangesAsync(token);

            return exerciseToChange;
        }
        public async Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default)
        {
            return _workoutsContext.Exercises.Any(e => e.Id == id && e.AddedByUserId == userId);
        }
    }
}
