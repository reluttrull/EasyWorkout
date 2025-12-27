using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class UpdateCompletedWorkoutRequest
    {
        public string? CompletedNotes { get; set; } = string.Empty;
    }
}
