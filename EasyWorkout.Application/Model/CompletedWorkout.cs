using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Application.Model
{
    public class CompletedWorkout
    {
        [Key]
        public required Guid Id { get; init; }
        public required Guid CompletedByUserId { get; init; }
        public Guid? WorkoutId { get; init; } // ok if original gets deleted
        [Required]
        [MaxLength(75)]
        public string FallbackName { get; set; } = string.Empty;
        public string Name => Workout?.Name ?? FallbackName; // existing workout name, or fallback name if deleted
        [Required]
        public required DateTime CompletedDate { get; init; }
        [MaxLength(250)]
        public string? CompletedNotes { get; set; }
        public string OriginalNotes => Workout?.Notes ?? string.Empty;
        public required DateTime LastEditedDate { get; set; }
        public virtual Workout? Workout { get; set; }
        public required List<CompletedExercise> CompletedExercises { get; init; } = [];
    }
}
