using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Model;
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

        public async Task<bool> DeleteAsync(Guid id, Guid userId, CancellationToken token = default)
        {
            var workoutToDelete = _workoutsContext.Workouts.Single(w => w.Id == id);
            if (workoutToDelete.AddedByUserId != userId) return false;

            _workoutsContext.Workouts.Remove(workoutToDelete);
            var result = await _workoutsContext.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<IEnumerable<Workout>> GetAllForUserAsync(Guid userId, CancellationToken token = default)
        {
            return _workoutsContext.Workouts
                .Where(w => w.AddedByUserId == userId)
                .AsEnumerable<Workout>();
        }

        public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _workoutsContext.Workouts
                .Where(w => w.Id == id)
                .First();
        }

        public async Task<Workout?> UpdateAsync(Guid id, UpdateWorkoutRequest request, Guid userId, CancellationToken token = default)
        {
            var workoutToChange = _workoutsContext.Workouts.First(w => w.Id == id);
            if (workoutToChange is null) return null;
            if (workoutToChange.AddedByUserId != userId) return null;

            workoutToChange.Name = request.Name;
            workoutToChange.Notes = request.Notes;

            await _workoutsContext.SaveChangesAsync(token);

            return workoutToChange;
        }
    }
}
