using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Responses
{
    public class CompletedExerciseSetResponse
    {
        public required Guid Id { get; init; }
        public required Guid? ExerciseSetId { get; init; }
        public required DateTime CompletedDate { get; init; }
        public required int SetNumber { get; set; }
        public int? Reps { get; set; }
        public int? GoalReps { get; set; }
        public double? Weight { get; set; }
        public double? GoalWeight { get; set; }
        public string? WeightUnit { get; set; }
        public double? Duration { get; set; }
        public double? GoalDuration { get; set; }
        public string? DurationUnit { get; set; }
        public double? Distance { get; set; }
        public double? GoalDistance { get; set; }
        public string? DistanceUnit { get; set; }

    }
}
