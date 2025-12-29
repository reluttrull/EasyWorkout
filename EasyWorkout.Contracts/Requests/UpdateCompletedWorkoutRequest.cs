using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class UpdateCompletedWorkoutRequest
    {
        [MaxLength(250)]
        public string? CompletedNotes { get; set; } = string.Empty;
    }
}
