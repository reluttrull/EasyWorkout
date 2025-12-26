using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Responses
{
    public class CompletedExerciseSetResponse
    {
        public required Guid Id { get; init; }
        public required Guid ExerciseSetId { get; init; }
        public required Guid CompletedWorkoutId { get; init; }
        public required DateTime CompletedDate { get; init; }
        public required int SetNumber { get; set; }
        public int? Reps { get; set; }
        public double? Weight { get; set; }
        public double? Duration { get; set; }
    }
}
