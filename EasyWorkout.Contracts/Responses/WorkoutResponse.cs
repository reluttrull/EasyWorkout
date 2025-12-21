using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Responses
{
    public class WorkoutResponse
    {
        public required Guid Id { get; init; }
        public required Guid AddedByUserId { get; init; }
        public required DateOnly AddedDate { get; init; }
        public required string Name { get; set; }
        public string? Notes { get; set; }
        public required IEnumerable<ExerciseResponse> Exercises { get; init; } = [];
    }
}
