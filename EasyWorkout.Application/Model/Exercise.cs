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
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }
        public List<Workout> Workouts { get; set; } = [];
        public List<ExerciseSet> ExerciseSets { get; init; } = [];
    }
}
