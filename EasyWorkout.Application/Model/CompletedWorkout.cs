using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Application.Model
{
    public class CompletedWorkout
    {
        [Key]
        public required Guid Id { get; init; }
        public required Guid CompletedByUserId { get; init; }
        public Guid WorkoutId { get; init; }
        [Required]
        public required DateOnly CompletedDate { get; init; }
        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;
        public required List<CompletedExercise> CompletedExercises { get; init; } = [];
    }
}
