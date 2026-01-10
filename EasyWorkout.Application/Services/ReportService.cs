using EasyWorkout.Application.Data;
using EasyWorkout.Application.Extensions;
using EasyWorkout.Application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly WorkoutsContext _workoutsContext;

        public ReportService(WorkoutsContext workoutsContext)
        {
            _workoutsContext = workoutsContext;
        }

        public async Task<IEnumerable<CompletedWorkout>> GetTotalVolumeForUserAsync(
            Guid userId, 
            DateTime? fromDate, 
            DateTime? toDate, 
            Guid? connectedToWorkoutId,
            CancellationToken token = default)
        {
            var filteredCompletedWorkouts =  await _workoutsContext.CompletedWorkouts
                .Where(cw => cw.CompletedByUserId == userId)
                .WhereIf(fromDate is not null, cw => cw.CompletedDate >= fromDate)
                .WhereIf(toDate is not null, cw => cw.CompletedDate <= toDate)
                .WhereIf(connectedToWorkoutId is not null, cw => cw.WorkoutId == connectedToWorkoutId)
                .Include(cw => cw.CompletedExercises)
                    .ThenInclude(ce => ce.CompletedExerciseSets)
                .OrderBy(cw => cw.CompletedDate)
                .ToListAsync();

            return filteredCompletedWorkouts;
        }
    }
}
