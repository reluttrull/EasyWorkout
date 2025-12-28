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
        public int ExerciseNumber { get; set; }
    }
}
