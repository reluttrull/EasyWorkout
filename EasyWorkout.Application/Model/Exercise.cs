using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Application.Model
{
    public class Exercise
    {
        [Key]
        public required Guid Id { get; init; }
        public required Guid AddedByUserId { get; init; }
        public required DateOnly AddedDate { get; init; }
        [Required]
        [MaxLength(75)]
        public required string Name { get; set; }
        [MaxLength(250)]
        public string? Notes { get; set; }
        public List<WorkoutExercise> WorkoutExercises { get; init; } = [];
        public List<ExerciseSet> ExerciseSets { get; init; } = [];
    }
}
