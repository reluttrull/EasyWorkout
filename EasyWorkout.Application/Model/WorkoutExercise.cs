using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Application.Model
{
    public class WorkoutExercise
    {
        [Key]
        public Guid Id { get; set; }
        public Guid WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;
        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;
        [Required]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int ExerciseNumber { get; set; }
    }
}
