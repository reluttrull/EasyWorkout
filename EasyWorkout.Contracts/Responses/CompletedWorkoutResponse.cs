using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Responses
{
    public class CompletedWorkoutResponse
    {
        public required Guid Id { get; init; }
        public required Guid CompletedByUserId { get; init; }
        public Guid? WorkoutId { get; init; }
        public string? OriginalName { get; init; }
        public string? OriginalNotes { get; init; }
        public required DateTime CompletedDate { get; init; }
        public DateTime LastEditedDate { get; init; }
        public string? CompletedNotes { get; set; }
        public required IEnumerable<CompletedExerciseResponse> CompletedExercises { get; init; } = [];
    }
}
