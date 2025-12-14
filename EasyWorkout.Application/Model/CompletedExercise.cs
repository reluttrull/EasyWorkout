using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class CompletedExercise
    {
        public required Guid CompletedExerciseSet { get; init; }
        public required Guid CompletedByUserId { get; init; }
        public Guid ExerciseId { get; init; }
        public required DateOnly CompletedDate { get; init; }
        public string Notes { get; set; } = string.Empty;
        public required List<CompletedExerciseSet> CompletedExerciseSets { get; init; } = [];
    }
}
