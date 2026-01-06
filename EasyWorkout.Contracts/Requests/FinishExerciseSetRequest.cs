using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Contracts.Requests
{
    public class FinishExerciseSetRequest
    {
        public Guid? ExerciseSetId { get; set; }
        public required DateTime CompletedDate { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public required int SetNumber { get; set; }
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? Reps { get; set; }
        [Range(0, 1200, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Weight { get; set; }
        [Range(0, 3600, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Duration { get; set; }
        [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Distance { get; set; }
        public int? GoalReps { get; set; }
        public double? GoalWeight { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WeightUnit? WeightUnit { get; set; }
        public double? GoalDuration { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DurationUnit? DurationUnit { get; set; }
        public double? GoalDistance { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DistanceUnit? DistanceUnit { get; set; }
    }
}