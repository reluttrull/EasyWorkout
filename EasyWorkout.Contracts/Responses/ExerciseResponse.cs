using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Responses
{
    public class ExerciseResponse
    {
        public required Guid Id { get; init; }
        public required Guid AddedByUserId { get; init; }
        public required DateOnly AddedDate { get; init; }
        public required int ExerciseNumber { get; set; }
        public required string Name { get; set; }
        public string? Notes { get; set; }
        public IEnumerable<ExerciseSetResponse> ExerciseSets { get; init; } = [];
    }
}
