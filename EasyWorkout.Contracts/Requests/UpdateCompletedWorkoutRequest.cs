using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class UpdateCompletedWorkoutRequest
    {
        [MaxLength(250, ErrorMessage = "{0} must be {1} characters or fewer.")]
        public string? CompletedNotes { get; set; } = string.Empty;
    }
}
