
using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Application.Model
{
    public class CompletedExerciseSet
    {
        [Key]
        public required Guid Id { get; init; }
        public required Guid CompletedExerciseId { get; init; }
        [Required]
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public required int SetNumber { get; set; }
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Reps { get; set; }
        [Range(0, 1200, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Weight { get; set; }
        public Enums.WeightUnit WeightUnit { get; set; }
        [Range(0, 3600, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Duration { get; set; }
        public Enums.DurationUnit DurationUnit { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;
    }
}
