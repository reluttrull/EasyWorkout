using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public interface IWorkoutService
    {
        Task<bool> CreateAsync(Workout workout, CancellationToken token = default);

        Task<Workout?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Workout>> GetAllForUserAsync(Guid userId, CancellationToken token = default);

        Task<Workout?> UpdateAsync(Guid id, UpdateWorkoutRequest request, Guid userId, CancellationToken token = default);

        Task<bool> DeleteAsync(Guid id, Guid userId, CancellationToken token = default);

        Task<bool> AddExerciseAsync(Guid id, Guid exerciseId, Guid userId, CancellationToken token = default);

        Task<bool> RemoveExerciseAsync(Guid id, Guid exerciseId, Guid userId, CancellationToken token = default);
    }
}
