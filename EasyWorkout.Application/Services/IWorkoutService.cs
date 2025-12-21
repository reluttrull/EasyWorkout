using EasyWorkout.Application.Model;
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
    }
}
