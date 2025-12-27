using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public interface ICompletedWorkoutService
    {
        Task<bool> CreateAsync(CompletedWorkout workout, CancellationToken token = default);

        Task<CompletedWorkout?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<CompletedWorkoutDetailed?> GetByIdDetailedAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<CompletedWorkout>> GetAllForUserAsync(Guid userId, CancellationToken token = default);

        Task<IEnumerable<CompletedWorkoutDetailed?>> GetAllDetailedForUserAsync(Guid userId, CancellationToken token = default);

        Task<CompletedWorkoutDetailed?> UpdateAsync(Guid id, UpdateCompletedWorkoutRequest request, CancellationToken token = default);

        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);

        Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default);
    }
}
