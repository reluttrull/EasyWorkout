using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class CompletedWorkout
    {
        public required Guid CompletedWorkoutId { get; init; }
        public required Guid CompletedByUserId { get; init; }
        public Guid WorkoutId { get; init; }
        public required DateOnly CompletedDate { get; init; }
        public string Notes { get; set; } = string.Empty;
        public required List<CompletedExercise> CompletedExercises { get; init; } = [];
    }
}
