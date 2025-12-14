using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class Workout
    {
        public required Guid WorkoutId { get; init; }
        public required Guid AddedByUserId { get; init; }
        public required DateOnly AddedDate { get; init; }
        public required string Name { get; set; }
        public string Notes { get; set; } = string.Empty;
        public required List<Exercise> Exercises { get; init; } = [];
    }
}
