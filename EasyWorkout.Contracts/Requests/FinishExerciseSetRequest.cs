using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class FinishExerciseSetRequest
    {
        public required Guid ExerciseSetId { get; set; }
        public required DateTime CompletedDate { get; set; }
        public required int SetNumber { get; set; }
        public int? Reps { get; set; }
        public double? Weight { get; set; }
        public double? Duration { get; set; }
    }
}
