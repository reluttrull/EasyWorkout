using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Responses
{
    public class CompletedExerciseResponse
    {
        public required Guid Id { get; init; }
        public required Guid ExerciseId { get; init; }
        public required Guid CompletedByUserId { get; init; }
        public string? Name { get; init; }
        public string? OriginalNotes { get; init; }
        public string? CompletedNotes { get; init; }
        public required DateTime CompletedDate { get; init; }
        public required IEnumerable<CompletedExerciseSetResponse> CompletedExerciseSets { get; init; } = [];
    }
}
