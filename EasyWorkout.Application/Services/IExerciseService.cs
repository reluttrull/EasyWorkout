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

        Task<IEnumerable<Exercise>> GetAllForUserAsync(Guid userId, CancellationToken token = default);

        Task<Exercise?> UpdateAsync(Guid id, UpdateExerciseRequest request, Guid userId, CancellationToken token = default);

        Task<bool> DeleteAsync(Guid id, Guid userId, CancellationToken token = default);

        Task<bool> CreateSetAsync(Guid id, ExerciseSet set, Guid userId, CancellationToken token = default);

        Task<bool> DeleteSetAsync(Guid id, Guid exerciseSetId,  Guid userId, CancellationToken token = default);
    }
}
