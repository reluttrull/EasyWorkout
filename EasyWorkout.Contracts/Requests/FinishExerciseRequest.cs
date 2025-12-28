using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class FinishExerciseRequest
    {
        public required Guid ExerciseId { get; set; }
        public required DateTime CompletedDate { get; set; }
        public required int ExerciseNumber { get; set; }
        public required List<FinishExerciseSetRequest> CompletedExerciseSets { get; set; }
    }
}
