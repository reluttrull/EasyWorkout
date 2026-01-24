using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class GetAllCompletedWorkoutsRequest : PagedRequest
    {
        public required DateTime? MinDate { get; init; }
        public required DateTime? MaxDate { get; init; }
        public required Guid? BasedOnWorkoutId { get; init; }
        public required Guid? ContainsExerciseId { get; init; }
        public required string? ContainsText { get; init; }
    }
}
