
using EasyWorkout.Contracts.Model;
using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Application.Model
{
    public class CompletedExerciseSet
    {
        [Key]
        public required Guid Id { get; init; }
        public required Guid? ExerciseSetId { get; init; } // ok if original gets deleted
        public required DateTime CompletedDate { get; init; }
        [Required]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public required int SetNumber { get; set; }
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? Reps { get; set; }
        public int? GoalReps { get; set; }
        [Range(0, 1200, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Weight { get; set; }
        public double? GoalWeight { get; set; }
        public Enums.WeightUnit? WeightUnit { get; set; }
        [Range(0, 3600, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Duration { get; set; }
        public double? GoalDuration { get; set; }
        public Enums.DurationUnit? DurationUnit { get; set; }
    }
}
