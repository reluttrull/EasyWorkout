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

        Task<WorkoutDetailed?> GetByIdDetailedAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<WorkoutDetailed>> GetAllDetailedForUserAsync(Guid userId, CancellationToken token = default);

        Task<Workout?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<Workout?> UpdateAsync(Guid id, UpdateWorkoutRequest request, CancellationToken token = default);

        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);

        Task<bool> DeleteAllAsync(Guid userId, CancellationToken token = default);

        Task<bool> AddExerciseAsync(Guid id, Guid exerciseId, CancellationToken token = default);

        Task<bool> RemoveExerciseAsync(Guid id, Guid exerciseId, CancellationToken token = default);

        Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default);

        Task<DateTime?> GetLastCompletedDateAsync(Guid id, CancellationToken token = default);
    }
}
