using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public interface IExerciseService
    {
        Task<bool> CreateAsync(Exercise exercise, CancellationToken token = default);

        Task<Exercise?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<ExerciseSet?> GetSetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Exercise>> GetAllForUserAsync(Guid userId, CancellationToken token = default);

        Task<Exercise?> UpdateAsync(Guid id, UpdateExerciseRequest request, CancellationToken token = default);

        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);

        Task<bool> DeleteAllAsync(Guid userId, CancellationToken token);

        Task<bool> CreateSetAsync(Guid id, ExerciseSet set, CancellationToken token = default);

        Task<bool> DeleteSetAsync(Guid id, Guid exerciseSetId, CancellationToken token = default);

        Task<bool> BelongsToUserAsync(Guid id, Guid userId, CancellationToken token = default);

        Task<int?> GetNextSetIndexAsync(Guid id, CancellationToken token = default);
    }
}
