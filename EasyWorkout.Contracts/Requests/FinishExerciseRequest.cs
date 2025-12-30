using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class FinishExerciseRequest
    {
        public required Guid ExerciseId { get; set; } // should start with one
        public required DateTime CompletedDate { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public required int ExerciseNumber { get; set; }
        public required List<FinishExerciseSetRequest> CompletedExerciseSets { get; set; }
    }
}
