using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class ExerciseSet
    {
        public required Guid ExerciseSetId { get; init; }
        //public required Guid ExerciseId { get; init; }
        public required int SetNumber { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public Enums.WeightUnit WeightUnit { get; set; }
        public double Duration { get; set; }
        public Enums.DurationUnit DurationUnit { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
