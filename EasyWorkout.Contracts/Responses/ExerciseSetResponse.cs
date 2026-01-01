using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Responses
{
    public class ExerciseSetResponse
    {
        public required Guid Id { get; init; }
        public required Guid ExerciseId { get; init; }
        public required int SetNumber { get; set; }
        public int? Reps { get; set; }
        public double? Weight { get; set; }
        public string? WeightUnit { get; set; }
        public double? Duration { get; set; }
        public string? DurationUnit { get; set; }
        public double? Distance { get; set; }
        public string? DistanceUnit { get; set; }
    }
}
