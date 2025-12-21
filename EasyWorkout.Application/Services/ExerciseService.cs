using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
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

        public async Task<bool> DeleteAsync(Guid id, Guid userId, CancellationToken token = default)
        {
            var exerciseToDelete = _workoutsContext.Exercises.Single(e => e.Id == id);
            if (exerciseToDelete.AddedByUserId != userId) return false;

            // todo: also delete all workout links

            // todo: also delete all dependent sets

            _workoutsContext.Exercises.Remove(exerciseToDelete);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<IEnumerable<Exercise>> GetAllForUserAsync(Guid userId, CancellationToken token = default)
        {
            return _workoutsContext.Exercises
                .Where(e => e.AddedByUserId == userId)
                .AsEnumerable<Exercise>();
        }

        public async Task<Exercise?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _workoutsContext.Exercises
                .First(e => e.Id == id);
        }

        public async Task<Exercise?> UpdateAsync(Guid id, UpdateExerciseRequest request, Guid userId, CancellationToken token = default)
        {
            var exerciseToChange = _workoutsContext.Exercises.First(e => e.Id == id);
            if (exerciseToChange is null) return null;
            if (exerciseToChange.AddedByUserId != userId) return null;

            exerciseToChange.Name = request.Name;
            exerciseToChange.Notes = request.Notes;

            await _workoutsContext.SaveChangesAsync(token);

            return exerciseToChange;
        }
    }
}
