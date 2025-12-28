using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class FinishWorkoutRequest
    {
        public Guid WorkoutId { get; set; }
        public required DateTime CompletedDate { get; set; }
        public string? CompletedNotes { get; set; } = string.Empty;
        public required List<FinishExerciseRequest> CompletedExercises { get; set; }
    }
}
