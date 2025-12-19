
using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Application.Model
{
    public class CompletedExercise
    {
        [Key]
        public required Guid Id { get; init; }
        public required Guid CompletedByUserId { get; init; }
        public required Guid CompletedWorkoutId { get; init; }
        public Guid ExerciseId { get; init; }
        [Required]
        public required DateOnly CompletedDate { get; init; }
        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;
        public required List<CompletedExerciseSet> CompletedExerciseSets { get; init; } = [];
    }
}
