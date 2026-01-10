using EasyWorkout.Application.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public interface IReportService
    {
        Task<IEnumerable<CompletedWorkout>> GetTotalVolumeForUserAsync(
            Guid userId,
            DateTime? fromDate,
            DateTime? toDate,
            Guid? connectedToWorkoutId,
            CancellationToken token = default);
    }
}
