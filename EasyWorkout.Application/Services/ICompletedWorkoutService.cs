using EasyWorkout.Application.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public interface ICompletedWorkoutService
    {
        Task<bool> CreateAsync(CompletedWorkout workout, CancellationToken token = default);

        Task<CompletedWorkout?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default);
    }
}
