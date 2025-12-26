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
        public required DateTime CompletedDate { get; init; }
        [MaxLength(250)]
        public string? CompletedNotes { get; set; }
        public required List<CompletedExerciseSet> CompletedExerciseSets { get; init; } = [];
    }
}
